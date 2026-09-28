using arc.app.Common;
using arc.common.ExtensionMethods;
using arc.common.Models.Reports.ReportDesigner;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.ReportsConfig;
using arc.domain.ExtensionMethods;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    /// <summary>
    /// Command for saving report designer configuration changes to the database.
    /// The whole save is one explicit transaction: formats are written first so that any rename forced by
    /// a name collision is known before the sections that reference them are written, then deletions are
    /// applied, and the report record is written last with the final section names.
    /// </summary>
    internal class SaveReportDesignerConfigCommand : ICommandWithTypeReturningType<SaveReportDesignerConfigModel, ReportDesignerSaveResultModel>
    {
        /// <summary>
        /// The ConfigTypeId used for report records.
        /// </summary>
        internal const int ReportConfigTypeId = ReportConfigTypes.Report;

        /// <summary>
        /// The ConfigTypeId used for main and final report section records.
        /// Only a fallback: a section is written to the type it was loaded from, and only a section
        /// that belongs to no known category lands here.
        /// </summary>
        internal const int SectionConfigTypeId = ReportConfigTypes.MainSection;

        /// <summary>
        /// The ConfigTypeId used for organism report section records.
        /// </summary>
        internal const int OrganismSectionConfigTypeId = ReportConfigTypes.OrganismSection;

        /// <summary>
        /// The ConfigTypeId used for report header records.
        /// </summary>
        internal const int HeaderConfigTypeId = ReportConfigTypes.ReportHeader;

        /// <summary>
        /// The ConfigTypeId used for report footer records.
        /// </summary>
        internal const int FooterConfigTypeId = ReportConfigTypes.ReportFooter;

        /// <summary>
        /// The ConfigTypeId used for report section format records.
        /// Formats previously shared type 21 with the reportstatus settings record, which made every
        /// designer load try to deserialise a settings row as a format.
        /// </summary>
        internal const int SectionFormatConfigTypeId = ReportConfigTypes.SectionFormat;

        /// <summary>
        /// The ConfigTypeId section formats were stored under before the split, still read as a fallback.
        /// </summary>
        internal const int LegacySectionFormatConfigTypeId = ReportConfigTypes.LegacySectionFormat;

        private static readonly JsonSerializerSettings SerializerSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        private readonly ConfigWriter _configWriter = new();

        /// <summary>
        /// Executes the command to save report designer configuration changes inside a single transaction.
        /// </summary>
        /// <param name="connect">The database connection. Opened here so every write shares one connection and one transaction.</param>
        /// <param name="command">The report designer configuration model to save.</param>
        /// <param name="logWriter">The log writer for logging operations.</param>
        /// <returns>A description of every configuration record written, so the designer can reconcile renames.</returns>
        public async Task<ReportDesignerSaveResultModel> ExecuteAsync(NpgsqlConnection connect, SaveReportDesignerConfigModel command, ILogWriter logWriter = null)
        {
            var result = new ReportDesignerSaveResultModel { ReportName = command?.Name };

            if (command == null)
            {
                logWriter?.LogError("Save called with a null configuration model.", nameof(SaveReportDesignerConfigCommand), nameof(ExecuteAsync));
                return result;
            }

            logWriter?.LogInfo(
                $"Saving report '{command.Name}': {command.ChangedCustomFormats?.Count ?? 0} format(s), {command.ChangedSectionDefinitions?.Count ?? 0} section(s), {command.DeletedFormats?.Count ?? 0} format deletion(s), {command.DeletedSections?.Count ?? 0} section deletion(s).",
                nameof(SaveReportDesignerConfigCommand), nameof(ExecuteAsync));

            await connect.OpenAsync();
            using var transaction = await connect.BeginTransactionAsync();

            try
            {
                var formatRenames = await ProcessFormatsAsync(connect, transaction, command, result, logWriter);

                ApplyFormatRenamesToSections(command, formatRenames, logWriter);

                await ProcessSectionsAsync(connect, transaction, command, result, logWriter);

                await ProcessDeletionsAsync(connect, transaction, command, result, logWriter);

                result.Report = await SaveReportRecordAsync(connect, transaction, command, logWriter);

                await transaction.CommitAsync();

                LogSummary(result, command.Name, logWriter);

                return result;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                logWriter?.LogError($"Rolled back the save of report '{command.Name}': {e.Message}", nameof(SaveReportDesignerConfigCommand), nameof(ExecuteAsync));
                throw;
            }
        }

        /// <summary>
        /// Writes every changed custom format and records any rename forced by a name collision.
        /// Runs before sections so that section format references can be corrected before they are stored.
        /// </summary>
        /// <param name="connect">The open database connection.</param>
        /// <param name="transaction">The transaction for the whole save.</param>
        /// <param name="command">The report designer configuration model.</param>
        /// <param name="result">The result being built, appended to as each format is written.</param>
        /// <param name="logWriter">The log writer.</param>
        /// <returns>A map of requested format name to stored format name for every format that was renamed.</returns>
        private async Task<Dictionary<string, string>> ProcessFormatsAsync(
            NpgsqlConnection connect,
            IDbTransaction transaction,
            SaveReportDesignerConfigModel command,
            ReportDesignerSaveResultModel result,
            ILogWriter logWriter)
        {
            var renames = new Dictionary<string, string>(StringComparer.Ordinal);

            foreach (var format in command.ChangedCustomFormats ?? [])
            {
                var requestedName = format?.Name.NormalisedConfigName();

                if (string.IsNullOrEmpty(requestedName))
                {
                    logWriter?.LogError("Skipped a custom format because it has no name.", nameof(SaveReportDesignerConfigCommand), nameof(ProcessFormatsAsync));
                    result.Formats.Add(new ConfigSaveResultModel
                    {
                        ConfigTypeId = SectionFormatConfigTypeId,
                        RequestedName = format?.Name,
                        State = format?.State ?? ConfigChangeState.Unchanged,
                        Action = "Skipped",
                        SkipReason = "The format has no name."
                    });
                    continue;
                }

                if (string.IsNullOrWhiteSpace(format.Description))
                {
                    logWriter?.LogError(
                        $"Skipped custom format '{requestedName}' because it has no description. The designer requires a description before a format can be stored.",
                        nameof(SaveReportDesignerConfigCommand), nameof(ProcessFormatsAsync));
                    result.Formats.Add(new ConfigSaveResultModel
                    {
                        ConfigTypeId = SectionFormatConfigTypeId,
                        RequestedName = requestedName,
                        State = format.State,
                        Action = "Skipped",
                        SkipReason = "The format has no description."
                    });
                    continue;
                }

                var state = Normalise(format.State);
                LogLineFontSizeNormalizations(format.Heading, $"format '{requestedName}' heading", logWriter);
                var persisted = format.ToPersistedFormat();
                persisted.Description = await PreserveLanguageTokenDescriptionAsync(
                    connect, transaction, format, requestedName, state, persisted.Description, logWriter);

                var writeResult = await _configWriter.UpsertAsync(
                    connect, transaction, format.ConfigId, requestedName, SectionFormatConfigTypeId,
                    JsonConvert.SerializeObject(persisted, SerializerSettings), state, logWriter);

                if (!writeResult.ConfigName.IsSameConfigName(requestedName))
                {
                    renames[requestedName] = writeResult.ConfigName;
                    logWriter?.LogInfo(
                        $"Format '{requestedName}' was stored as '{writeResult.ConfigName}'; sections referencing it will be repointed before they are written.",
                        nameof(SaveReportDesignerConfigCommand), nameof(ProcessFormatsAsync));
                }

                result.Formats.Add(new ConfigSaveResultModel
                {
                    ConfigId = writeResult.ConfigId,
                    ConfigTypeId = SectionFormatConfigTypeId,
                    RequestedName = requestedName,
                    SavedName = writeResult.ConfigName,
                    State = state,
                    Action = writeResult.Action
                });
            }

            return renames;
        }

        /// <summary>
        /// Keeps an existing format's stored description when it is a @Tag@ language token.
        /// </summary>
        /// <remarks>
        /// The designer is served a translated description, so saving what it sends back would replace
        /// the token with the text of whichever language the editing user happened to be using and break
        /// the format's label for every other language. The token is therefore kept unless the incoming
        /// description is itself a token, which is the only way a caller can deliberately change it.
        /// </remarks>
        /// <param name="connect">The open database connection.</param>
        /// <param name="transaction">The transaction for the whole save.</param>
        /// <param name="format">The format being written.</param>
        /// <param name="requestedName">The normalised format name.</param>
        /// <param name="state">The change requested for the format.</param>
        /// <param name="incomingDescription">The description the designer sent.</param>
        /// <param name="logWriter">The log writer.</param>
        /// <returns>The description to store.</returns>
        private async Task<string> PreserveLanguageTokenDescriptionAsync(
            NpgsqlConnection connect,
            IDbTransaction transaction,
            CustomFormatModel format,
            string requestedName,
            ConfigChangeState state,
            string incomingDescription,
            ILogWriter logWriter)
        {
            if (state == ConfigChangeState.Added || IsLanguageToken(incomingDescription))
            {
                return incomingDescription;
            }

            var storedContents = await _configWriter.ReadContentsAsync(
                connect, transaction, format.ConfigId, requestedName, SectionFormatConfigTypeId);

            if (string.IsNullOrWhiteSpace(storedContents))
            {
                return incomingDescription;
            }

            var storedDescription = JsonConvert.DeserializeObject<ReportSectionFormatConfig>(storedContents)?.Description;

            if (!IsLanguageToken(storedDescription))
            {
                return incomingDescription;
            }

            logWriter?.LogInfo(
                $"Kept the language token description '{storedDescription}' on format '{requestedName}' rather than storing the translated text '{incomingDescription}'.",
                nameof(SaveReportDesignerConfigCommand), nameof(PreserveLanguageTokenDescriptionAsync));

            return storedDescription;
        }

        /// <summary>
        /// Determines whether a value is a language token of the form @Tag@ rather than display text.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>True when the value is a language token.</returns>
        private static bool IsLanguageToken(string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                   && value.Length > 2
                   && value.StartsWith('@')
                   && value.EndsWith('@');
        }

        /// <summary>
        /// Repoints section format references at the names the formats were actually stored under.
        /// Also applied to the sections held in the report categories so the report record stays consistent.
        /// </summary>
        /// <param name="command">The report designer configuration model, updated in place.</param>
        /// <param name="formatRenames">Map of requested format name to stored format name.</param>
        /// <param name="logWriter">The log writer.</param>
        private static void ApplyFormatRenamesToSections(
            SaveReportDesignerConfigModel command,
            Dictionary<string, string> formatRenames,
            ILogWriter logWriter)
        {
            if (formatRenames.Count == 0)
            {
                return;
            }

            foreach (var section in command.ChangedSectionDefinitions ?? [])
            {
                var currentFormat = section?.Format.NormalisedConfigName();
                if (!string.IsNullOrEmpty(currentFormat) && formatRenames.TryGetValue(currentFormat, out var storedName))
                {
                    section.Format = storedName;
                    logWriter?.LogInfo(
                        $"Section '{section.Name}' now references format '{storedName}'.",
                        nameof(SaveReportDesignerConfigCommand), nameof(ApplyFormatRenamesToSections));
                }
            }
        }

        /// <summary>
        /// Writes every changed section definition and repoints the report categories at the stored names.
        /// </summary>
        /// <param name="connect">The open database connection.</param>
        /// <param name="transaction">The transaction for the whole save.</param>
        /// <param name="command">The report designer configuration model.</param>
        /// <param name="result">The result being built, appended to as each section is written.</param>
        /// <param name="logWriter">The log writer.</param>
        private async Task ProcessSectionsAsync(
            NpgsqlConnection connect,
            IDbTransaction transaction,
            SaveReportDesignerConfigModel command,
            ReportDesignerSaveResultModel result,
            ILogWriter logWriter)
        {
            var formatGridCapacity = BuildFormatGridCapacityMap(command.ChangedCustomFormats);
            var formatColumnCapacity = SectionFormatReconciliationExtensions.BuildFormatColumnCapacityMap(command.ChangedCustomFormats);
            var formatsByName = SectionFormatReconciliationExtensions.BuildFormatMap(command.ChangedCustomFormats);

            foreach (var section in command.ChangedSectionDefinitions ?? [])
            {
                WarnIfMissingSaveScopeForSharedHeaderFooter(section, command, logWriter);

                await ApplyHeaderFooterForkIfRequestedAsync(connect, transaction, command, section, logWriter);

                var requestedName = section?.Name.NormalisedConfigName();

                if (string.IsNullOrEmpty(requestedName))
                {
                    logWriter?.LogError("Skipped a section definition because it has no name.", nameof(SaveReportDesignerConfigCommand), nameof(ProcessSectionsAsync));
                    result.Sections.Add(new ConfigSaveResultModel
                    {
                        ConfigTypeId = SectionConfigTypeId,
                        RequestedName = section?.Name,
                        State = section?.State ?? ConfigChangeState.Unchanged,
                        Action = "Skipped",
                        SkipReason = "The section has no name."
                    });
                    continue;
                }

                var state = Normalise(section.State);
                var configTypeId = ResolveSectionConfigTypeId(section.ConfigTypeId, section.Scope, requestedName, command);

                logWriter?.LogInfo(
                    $"Writing section '{requestedName}' to ConfigType {configTypeId} (id {section.ConfigId?.ToString() ?? "none"}, scope '{section.Scope ?? "none"}').",
                    nameof(SaveReportDesignerConfigCommand), nameof(ProcessSectionsAsync));

                TrimSectionGridsToFormatCapacity(section, formatGridCapacity, logWriter);

                var originalHeadingText = section.HeadingText;
                section.HeadingText = section.HeadingText.NormalizeSectionHeading();
                if (originalHeadingText != section.HeadingText)
                {
                    logWriter?.LogInfo(
                        $"Section '{section.Name}' HeadingText normalized from '{originalHeadingText ?? "(null)"}' to empty (no section heading).",
                        nameof(SaveReportDesignerConfigCommand), nameof(ProcessSectionsAsync));
                }

                var removedFieldCount = SectionFormatReconciliationExtensions.TrimSectionFieldsToFormatCapacity(
                    section, formatColumnCapacity);
                if (removedFieldCount > 0)
                {
                    var formatName = section.Format.NormalisedConfigName();
                    formatColumnCapacity.TryGetValue(formatName, out var columnCapacity);
                    logWriter?.LogInfo(
                        $"Section '{section.Name}' had {removedFieldCount} field(s) beyond format '{formatName}' column capacity ({columnCapacity}); trimmed before save.",
                        nameof(SaveReportDesignerConfigCommand), nameof(ProcessSectionsAsync));
                }

                var removedHeaderCount = SectionFormatReconciliationExtensions.TrimSectionGridHeadsToFormatCapacity(
                    section, formatsByName);
                if (removedHeaderCount > 0)
                {
                    logWriter?.LogInfo(
                        $"Section '{section.Name}' had {removedHeaderCount} excess grid header slot(s) for format '{section.Format}'; trimmed before save.",
                        nameof(SaveReportDesignerConfigCommand), nameof(ProcessSectionsAsync));
                }

                LogLineFontSizeNormalizations(section.Lines, $"section '{section.Name}'", logWriter);

                var writeResult = await _configWriter.UpsertAsync(
                    connect, transaction, section.ConfigId, requestedName, configTypeId,
                    JsonConvert.SerializeObject(section.ToPersistedSection(), SerializerSettings), state, logWriter);

                if (!writeResult.ConfigName.IsSameConfigName(requestedName))
                {
                    RenameSectionInCategories(command, requestedName, writeResult.ConfigName);
                    UpdateHeaderFooterReferenceIfNeeded(command, section, requestedName, writeResult.ConfigName);
                    logWriter?.LogInfo(
                        $"Section '{requestedName}' was stored as '{writeResult.ConfigName}'; the report categories have been repointed.",
                        nameof(SaveReportDesignerConfigCommand), nameof(ProcessSectionsAsync));
                }

                if (section.RequiresFork())
                {
                    logWriter?.LogInfo(
                        $"Forked header/footer for report '{command.Name}': stored as '{writeResult.ConfigName}' (id {writeResult.ConfigId}, action {writeResult.Action}).",
                        nameof(SaveReportDesignerConfigCommand), nameof(ProcessSectionsAsync));
                }

                result.Sections.Add(new ConfigSaveResultModel
                {
                    ConfigId = writeResult.ConfigId,
                    ConfigTypeId = configTypeId,
                    RequestedName = requestedName,
                    SavedName = writeResult.ConfigName,
                    State = state,
                    Action = writeResult.Action
                });
            }
        }

        /// <summary>
        /// Logs when a shared header or footer change arrives without an explicit save scope.
        /// </summary>
        /// <param name="section">The section being written.</param>
        /// <param name="command">The report designer configuration model.</param>
        /// <param name="logWriter">The log writer.</param>
        private static void WarnIfMissingSaveScopeForSharedHeaderFooter(
            ReportSectionModel section,
            SaveReportDesignerConfigModel command,
            ILogWriter logWriter)
        {
            if (section == null || !section.Scope.IsHeaderOrFooterScope())
            {
                return;
            }

            var linkedCount = section.LinkedReportCount ?? 0;
            if (linkedCount <= 1 || !string.IsNullOrWhiteSpace(section.SaveScope))
            {
                return;
            }

            logWriter?.LogWarning(
                $"Header/footer '{section.Name}' is linked to {linkedCount} report(s) but SaveScope was not supplied for report '{command?.Name}'; defaulting to shared update.",
                nameof(SaveReportDesignerConfigCommand), nameof(WarnIfMissingSaveScopeForSharedHeaderFooter));
        }

        /// <summary>
        /// Forks a header or footer for the current report by inserting under a new name and relinking the report.
        /// </summary>
        /// <param name="connect">The open database connection.</param>
        /// <param name="transaction">The transaction for the whole save.</param>
        /// <param name="command">The report designer configuration model, updated in place.</param>
        /// <param name="section">The header or footer section being written.</param>
        /// <param name="logWriter">The log writer.</param>
        private static async Task ApplyHeaderFooterForkIfRequestedAsync(
            NpgsqlConnection connect,
            IDbTransaction transaction,
            SaveReportDesignerConfigModel command,
            ReportSectionModel section,
            ILogWriter logWriter)
        {
            if (section == null || !section.RequiresFork())
            {
                return;
            }

            var originalName = section.Name.NormalisedConfigName();
            var queryFilters = new QueryFilterConfig()
                .AddString("name", originalName)
                .AddString("suffix", string.Empty);

            var forkedName = await new NextFreeNameQuery().ExecuteAsync(connect, queryFilters, transaction);

            logWriter?.LogInfo(
                $"Forking {section.Scope.ToLower()} '{originalName}' for report '{command?.Name}' (linked to {section.LinkedReportCount ?? 0} report(s)) as '{forkedName}'.",
                nameof(SaveReportDesignerConfigCommand), nameof(ApplyHeaderFooterForkIfRequestedAsync));

            section.Name = forkedName;
            section.ConfigId = null;
            section.State = ConfigChangeState.Added;

            if (section.Scope.IsSameConfigName(ConfigScope.Header)
                && command.Header.IsSameConfigName(originalName))
            {
                command.Header = forkedName;
            }
            else if (section.Scope.IsSameConfigName(ConfigScope.Footer)
                     && command.Footer.IsSameConfigName(originalName))
            {
                command.Footer = forkedName;
            }
        }

        /// <summary>
        /// Repoints the report header or footer reference when a section was stored under a different name.
        /// </summary>
        /// <param name="command">The report designer configuration model, updated in place.</param>
        /// <param name="section">The section that was written.</param>
        /// <param name="requestedName">The name the designer sent.</param>
        /// <param name="storedName">The name the section was stored under.</param>
        private static void UpdateHeaderFooterReferenceIfNeeded(
            SaveReportDesignerConfigModel command,
            ReportSectionModel section,
            string requestedName,
            string storedName)
        {
            if (section == null || !section.Scope.IsHeaderOrFooterScope())
            {
                return;
            }

            if (section.Scope.IsSameConfigName(ConfigScope.Header)
                && command.Header.IsSameConfigName(requestedName))
            {
                command.Header = storedName;
            }
            else if (section.Scope.IsSameConfigName(ConfigScope.Footer)
                     && command.Footer.IsSameConfigName(requestedName))
            {
                command.Footer = storedName;
            }
        }

        /// <summary>
        /// Decides which ConfigTypeId a section definition must be written to.
        /// </summary>
        /// <remarks>
        /// Report sections do not all share one type: organism sections have their own, and headers and
        /// footers have theirs. Because <see cref="ConfigWriter"/> scopes both its identity lookup and
        /// its name fallback by type, writing a section to the wrong type matches nothing and inserts a
        /// renamed duplicate instead of updating the record the designer loaded. The type the designer
        /// sends back is therefore preferred over anything inferred here, and inference is only for a
        /// section the designer has just created and which has no record yet.
        /// </remarks>
        /// <param name="configTypeId">The type the designer loaded the section from, when it loaded one.</param>
        /// <param name="scope">The part of the report the section belongs to, for a newly created section.</param>
        /// <param name="requestedName">The normalised section name, used to find the owning category.</param>
        /// <param name="command">The report designer configuration model, which carries the categories.</param>
        /// <returns>The ConfigTypeId to write the section to.</returns>
        private static int ResolveSectionConfigTypeId(
            int? configTypeId,
            string scope,
            string requestedName,
            SaveReportDesignerConfigModel command)
        {
            if (configTypeId.HasValue && configTypeId.Value > 0)
            {
                return configTypeId.Value;
            }

            if (scope.IsSameConfigName(ConfigScope.Header))
            {
                return HeaderConfigTypeId;
            }

            if (scope.IsSameConfigName(ConfigScope.Footer))
            {
                return FooterConfigTypeId;
            }

            var owningCategory = (command?.Categories ?? [])
                .FirstOrDefault(category => category?.Sections?
                    .Any(sectionName => sectionName.IsSameConfigName(requestedName)) == true);

            var isOrganism = owningCategory?.Source.IsSameConfigName("Organism") == true
                             || owningCategory?.SourceName.IsSameConfigName("OrganismSections") == true;

            return isOrganism ? OrganismSectionConfigTypeId : SectionConfigTypeId;
        }

        /// <summary>
        /// Builds a map of format name to grid position count from the formats included in the same save.
        /// </summary>
        /// <param name="changedFormats">The custom formats the designer changed in this save.</param>
        /// <returns>Normalised format name to the number of grid positions the format defines.</returns>
        private static Dictionary<string, int> BuildFormatGridCapacityMap(IEnumerable<CustomFormatModel> changedFormats)
        {
            var capacity = new Dictionary<string, int>(StringComparer.Ordinal);

            foreach (var format in changedFormats ?? [])
            {
                var formatName = format?.Name.NormalisedConfigName();
                if (string.IsNullOrEmpty(formatName))
                {
                    continue;
                }

                capacity[formatName] = format.Grids?.Count ?? 0;
            }

            return capacity;
        }

        /// <summary>
        /// Trims section grid bindings when they exceed the format capacity supplied in the same save payload.
        /// </summary>
        /// <param name="section">The section being written.</param>
        /// <param name="formatGridCapacity">Grid position counts for formats changed in this save.</param>
        /// <param name="logWriter">The log writer.</param>
        private static void TrimSectionGridsToFormatCapacity(
            ReportSectionModel section,
            Dictionary<string, int> formatGridCapacity,
            ILogWriter logWriter)
        {
            if (section == null || section.Grids == null || section.Grids.Count == 0)
            {
                return;
            }

            var formatName = section.Format.NormalisedConfigName();
            if (string.IsNullOrEmpty(formatName) || !formatGridCapacity.TryGetValue(formatName, out var capacity))
            {
                return;
            }

            if (section.Grids.Count <= capacity)
            {
                return;
            }

            var bindingCount = section.Grids.Count;
            section.Grids = section.Grids.Take(capacity).ToList();

            logWriter?.LogInfo(
                $"Section '{section.Name}' binds {bindingCount} grid(s) but format '{formatName}' defines {capacity} position(s); trimmed to {capacity} binding(s) before save.",
                nameof(SaveReportDesignerConfigCommand), nameof(TrimSectionGridsToFormatCapacity));
        }

        /// <summary>
        /// Removes the configuration records for sections and formats the designer deleted.
        /// </summary>
        /// <param name="connect">The open database connection.</param>
        /// <param name="transaction">The transaction for the whole save.</param>
        /// <param name="command">The report designer configuration model.</param>
        /// <param name="result">The result being built, appended to as each record is removed.</param>
        /// <param name="logWriter">The log writer.</param>
        private async Task ProcessDeletionsAsync(
            NpgsqlConnection connect,
            IDbTransaction transaction,
            SaveReportDesignerConfigModel command,
            ReportDesignerSaveResultModel result,
            ILogWriter logWriter)
        {
            foreach (var deletedFormat in command.DeletedFormats ?? [])
            {
                var writeResult = await _configWriter.DeleteAsync(
                    connect, transaction, deletedFormat?.ConfigId, deletedFormat?.Name, SectionFormatConfigTypeId, logWriter);

                if (writeResult.Action == "NotFound")
                {
                    var legacyResult = await _configWriter.DeleteAsync(
                        connect, transaction, deletedFormat?.ConfigId, deletedFormat?.Name, LegacySectionFormatConfigTypeId, logWriter);
                    if (legacyResult.Action == "Deleted")
                    {
                        writeResult = legacyResult;
                    }
                }

                result.Formats.Add(new ConfigSaveResultModel
                {
                    ConfigId = writeResult.ConfigId,
                    ConfigTypeId = SectionFormatConfigTypeId,
                    RequestedName = deletedFormat?.Name,
                    SavedName = writeResult.ConfigName,
                    State = ConfigChangeState.Deleted,
                    Action = writeResult.Action
                });
            }

            foreach (var deletedSection in command.DeletedSections ?? [])
            {
                var configTypeId = ResolveSectionConfigTypeId(
                    deletedSection?.ConfigTypeId,
                    deletedSection?.Scope,
                    deletedSection?.Name.NormalisedConfigName(),
                    command);

                var writeResult = await _configWriter.DeleteAsync(
                    connect, transaction, deletedSection?.ConfigId, deletedSection?.Name, configTypeId, logWriter);

                result.Sections.Add(new ConfigSaveResultModel
                {
                    ConfigId = writeResult.ConfigId,
                    ConfigTypeId = configTypeId,
                    RequestedName = deletedSection?.Name,
                    SavedName = writeResult.ConfigName,
                    State = ConfigChangeState.Deleted,
                    Action = writeResult.Action
                });
            }
        }

        /// <summary>
        /// Writes the report record itself, last, so it carries the final section names.
        /// </summary>
        /// <param name="connect">The open database connection.</param>
        /// <param name="transaction">The transaction for the whole save.</param>
        /// <param name="command">The report designer configuration model.</param>
        /// <param name="logWriter">The log writer.</param>
        /// <returns>The outcome of writing the report record.</returns>
        private async Task<ConfigSaveResultModel> SaveReportRecordAsync(
            NpgsqlConnection connect,
            IDbTransaction transaction,
            SaveReportDesignerConfigModel command,
            ILogWriter logWriter)
        {
            var reportConfig = ConvertToReportConfig(command);
            var contents = JsonConvert.SerializeObject(reportConfig, SerializerSettings);

            var writeResult = await _configWriter.UpsertAsync(
                connect, transaction, null, reportConfig.Name, ReportConfigTypeId, contents, ConfigChangeState.Modified, logWriter);

            return new ConfigSaveResultModel
            {
                ConfigId = writeResult.ConfigId,
                ConfigTypeId = ReportConfigTypeId,
                RequestedName = reportConfig.Name,
                SavedName = writeResult.ConfigName,
                State = ConfigChangeState.Modified,
                Action = writeResult.Action
            };
        }

        /// <summary>
        /// Replaces a section name everywhere it appears in the report categories.
        /// </summary>
        /// <param name="command">The report designer configuration model, updated in place.</param>
        /// <param name="originalName">The name the designer sent.</param>
        /// <param name="storedName">The name the section was actually stored under.</param>
        private static void RenameSectionInCategories(SaveReportDesignerConfigModel command, string originalName, string storedName)
        {
            foreach (var category in command.Categories ?? [])
            {
                if (category?.Sections == null)
                {
                    continue;
                }

                for (var index = 0; index < category.Sections.Count; index++)
                {
                    if (category.Sections[index].IsSameConfigName(originalName))
                    {
                        category.Sections[index] = storedName;
                    }
                }
            }
        }

        /// <summary>
        /// Treats an item that arrived in a change list without an explicit state as a modification,
        /// so a missing state can never cause a change to be silently dropped.
        /// </summary>
        /// <param name="state">The state supplied by the designer.</param>
        /// <returns>The state to act on.</returns>
        private static ConfigChangeState Normalise(ConfigChangeState state)
        {
            return state == ConfigChangeState.Unchanged ? ConfigChangeState.Modified : state;
        }

        /// <summary>
        /// Converts the SaveReportDesignerConfigModel to a ReportConfig for database storage.
        /// </summary>
        /// <param name="config">The save configuration model.</param>
        /// <returns>A ReportConfig instance.</returns>
        private static ReportConfig ConvertToReportConfig(SaveReportDesignerConfigModel config)
        {
            var categories = config.Categories ?? [];

            return new ReportConfig
            {
                Name = config.Name.NormalisedConfigName(),
                Title = config.Title,
                Header = config.Header,
                Footer = config.Footer,
                IncludeAlerts = config.IncludeAlerts,
                Configurable = config.Enabled,
                SectionSource = BuildSectionSources(config.SectionSources),
                MainSections = GetSectionsBySource(categories, "MainSections"),
                OrganismSections = GetSectionsBySource(categories, "OrganismSections"),
                FinalSections = GetSectionsBySource(categories, "FinalSections")
            };
        }

        /// <summary>
        /// Gets sections for a specific source name from the categories list.
        /// </summary>
        /// <param name="categories">The list of categories.</param>
        /// <param name="sourceName">The source name to find (e.g., "MainSections", "OrganismSections", "FinalSections").</param>
        /// <returns>The list of section names for the specified source, or an empty list if not found.</returns>
        private static List<string> GetSectionsBySource(IEnumerable<ReportCategoryModel> categories, string sourceName)
        {
            var category = categories?.FirstOrDefault(c => c.SourceName.IsSameConfigName(sourceName));
            return category?.Sections ?? [];
        }

        /// <summary>
        /// Builds the section sources for the report, using the set the designer sent and falling back to
        /// the fixed Main, Organism and Final set when the designer sent none.
        /// </summary>
        /// <param name="sectionSources">The section sources supplied by the designer.</param>
        /// <returns>The section sources to store on the report.</returns>
        private static List<SectionSource> BuildSectionSources(List<SectionSourceModel> sectionSources)
        {
            if (sectionSources != null && sectionSources.Count > 0)
            {
                return sectionSources
                    .Where(source => !string.IsNullOrWhiteSpace(source?.Name))
                    .Select(source => new SectionSource { Name = source.Name, Source = source.Source })
                    .ToList();
            }

            return
            [
                new SectionSource { Name = "MainSections", Source = "Main" },
                new SectionSource { Name = "OrganismSections", Source = "Organism" },
                new SectionSource { Name = "FinalSections", Source = "Main" }
            ];
        }

        /// <summary>
        /// Writes a single summary line describing everything the transaction did.
        /// </summary>
        /// <param name="result">The completed save result.</param>
        /// <param name="reportName">The report that was saved.</param>
        /// <param name="logWriter">The log writer.</param>
        private static void LogSummary(ReportDesignerSaveResultModel result, string reportName, ILogWriter logWriter)
        {
            var all = result.Formats.Concat(result.Sections).ToList();
            if (result.Report != null)
            {
                all.Add(result.Report);
            }

            logWriter?.LogInfo(
                $"Committed save of report '{reportName}': {Count(all, "Inserted")} inserted, {Count(all, "Updated")} updated, {Count(all, "Deleted")} deleted, {Count(all, "Skipped")} skipped, {Count(all, "NotFound")} not found.",
                nameof(SaveReportDesignerConfigCommand), nameof(ExecuteAsync));
        }

        private static int Count(IEnumerable<ConfigSaveResultModel> results, string action)
        {
            return results.Count(item => string.Equals(item.Action, action, StringComparison.Ordinal));
        }

        /// <summary>
        /// Clamps out-of-range line FontSize values before persistence and logs each adjustment.
        /// </summary>
        /// <param name="lines">Line elements to normalize.</param>
        /// <param name="context">Description of the owning config record for log messages.</param>
        /// <param name="logWriter">The log writer.</param>
        private static void LogLineFontSizeNormalizations(
            IList<ReportLineModel> lines,
            string context,
            ILogWriter logWriter)
        {
            foreach (var message in lines.NormalizeReportLineFontSizes(context))
            {
                logWriter?.LogInfo(message, nameof(SaveReportDesignerConfigCommand), nameof(LogLineFontSizeNormalizations));
            }
        }
    }
}
