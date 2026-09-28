using arc.app.Common;
using arc.app.Config;
using arc.app.Config.Reports;
using arc.app.Configuration;
using arc.app.Config.Reports.DataSection;
using arc.common;
using arc.common.ExtensionMethods;
using arc.common.Models.Common;
using arc.common.Models.Reports.ReportDesigner;
using arc.domain.Configuration.ReportsConfig;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace arc.app.Reports.ReportDesigner;

/// <summary>
/// Represents view information including name and available datasections.
/// </summary>
public class ViewInfo
{
    public string ViewName { get; set; }
    public List<ReportCategoryAvailabilityConfig> ReportCategories { get; set; }
    public string AllowedHeaders { get; set; }
    public string AllowedFooters { get; set; }
}

/// <summary>
/// Handler for report designer configuration operations.
/// </summary>
public class ReportDesignerHandler(IConfigCache configCache, IReportAdapter reportAdapter, IListViewConfigFactory listViewConfigFactory, IMapType<(ReportConfig reportConfig, ViewInfo viewInfo), ReportDesignerModel> reportDesignerMapper, ISectionAdapter sectionAdapter, IMapType<ReportSectionConfig, ReportSectionModel> sectionMapper, IDataSectionAdapter dataSectionAdapter, IReportHeaderAdapter reportHeaderAdapter, IReportFooterAdapter reportFooterAdapter, IMapType<ReportSectionFormatConfig, CustomFormatModel> customFormatMapper, IMapType<(ReportHeaderFooterConfig config, string sectionType), ReportSectionModel> headerFooterMapper, IReportHeaderFooterUsageService headerFooterUsageService, ILogWriter logWriter) : IReportDesignerHandler
{
    /// <summary>
    /// The ConfigTypeId section formats are stored under.
    /// </summary>
    private const int SectionFormatConfigTypeId = ReportConfigTypes.SectionFormat;

    /// <summary>
    /// The ConfigTypeId section formats were stored under before they were split away from the shared
    /// settings type, still read as a fallback for databases that have not run the migration.
    /// </summary>
    private const int LegacySectionFormatConfigTypeId = ReportConfigTypes.LegacySectionFormat;

    /// <summary>
    /// Asynchronously retrieves the report designer configuration for the specified report.
    /// </summary>
    /// <param name="reportId">The report identifier containing the report name.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the report designer model.</returns>
    public async Task<ReportDesignerModel> GetReportDesignerConfiguration(IdAndNameModel reportId)
    {
        logWriter.LogInfo($"Getting report designer configuration for report: {reportId.Name}", nameof(ReportDesignerHandler), nameof(GetReportDesignerConfiguration));

        // Load the configuration cache if not already loaded
        if (!configCache.isLoaded())
        {
            logWriter.LogInfo("Loading configuration cache", nameof(ReportDesignerHandler), nameof(GetReportDesignerConfiguration));
            await configCache.LoadConfig();
        }

        // Get the report configuration using the report name from the IdAndNameModel
        logWriter.LogInfo($"Retrieving report configuration for: {reportId.Name}", nameof(ReportDesignerHandler), nameof(GetReportDesignerConfiguration));
        var reportConfig = await reportAdapter.GetReportAsync(reportId.Name);

        // Find which view contains this report
        logWriter.LogInfo($"Finding view for report: {reportId.Name}", nameof(ReportDesignerHandler), nameof(GetReportDesignerConfiguration));
        var viewInfo = await FindViewContainingReportAsync(reportId.Name);

        // Map the report configuration and view information to ReportDesignerModel
        logWriter.LogInfo($"Mapping report configuration to ReportDesignerModel for: {reportId.Name}", nameof(ReportDesignerHandler), nameof(GetReportDesignerConfiguration));
        var reportDesignerModel = reportDesignerMapper.Map((reportConfig, viewInfo));

        if (viewInfo?.ReportCategories != null && viewInfo.ReportCategories.Count > 0)
        {
            reportDesignerModel.References.Clear();

            foreach (var viewCategory in viewInfo.ReportCategories)
            {
                var reference = new ReportReferenceModel
                {
                    Name = viewCategory.Name,
                    AvailableDataSections = ParseDataSections(viewCategory.DataSections),
                    AvailableFields = new List<ReportAvailableFieldModel>()
                };

                reportDesignerModel.References.Add(reference);
            }
        }

        if (reportConfig.SectionSource != null)
        {
            reportDesignerModel.Categories.Clear();

            foreach (var sectionSource in reportConfig.SectionSource)
            {
                var category = new ReportCategoryModel
                {
                    Name = GetCategoryDisplayName(sectionSource.Name),
                    Type = ResolveCategoryType(sectionSource.Name, sectionSource.Source),
                    SourceName = sectionSource.Name,
                    Source = sectionSource.Source,
                    Sections = new List<string>()
                };

                var sectionList = GetSectionListByName(reportConfig, sectionSource.Name);
                if (sectionList != null)
                {
                    category.Sections = sectionList.ToList();
                }

                reportDesignerModel.Categories.Add(category);
            }
        }

        // Load section definitions for all sections included in the categories
        logWriter.LogInfo($"Loading section definitions for report: {reportId.Name}", nameof(ReportDesignerHandler), nameof(GetReportDesignerConfiguration));
        var sectionDefinitions = await LoadSectionDefinitionsAsync(reportDesignerModel.Categories);
        reportDesignerModel.SectionDefinitions = sectionDefinitions;

        // Load available fields for each reference based on available sections' data sections
        logWriter.LogInfo($"Loading available fields for references for report: {reportId.Name}", nameof(ReportDesignerHandler), nameof(GetReportDesignerConfiguration));
        await PopulateAvailableFieldsAsync(reportDesignerModel.References);

        // Load custom formats from section format adapter
        logWriter.LogInfo($"Loading custom formats", nameof(ReportDesignerHandler), nameof(GetReportDesignerConfiguration));
        reportDesignerModel.CustomFormats = await LoadCustomFormatsAsync();

        // Add calculated fields
        reportDesignerModel.CalculatedFields =
        [
            new ReportAvailableFieldModel { Name = "PrintedDate", Label = "Printed Date" },
            new ReportAvailableFieldModel { Name = "Pages", Label = "Pages" }
        ];

        // Populate allowed headers and footers from view information
        if (viewInfo != null)
        {
            reportDesignerModel.AllowedHeaders = viewInfo.AllowedHeaders;
            reportDesignerModel.AllowedFooters = viewInfo.AllowedFooters;
        }

        // Populate header section definitions from allowed headers
        reportDesignerModel.HeaderSectionDefinitions = await LoadHeaderSectionDefinitionsAsync(reportDesignerModel.AllowedHeaders);

        // Populate footer section definitions from allowed footers
        reportDesignerModel.FooterSectionDefinitions = await LoadFooterSectionDefinitionsAsync(reportDesignerModel.AllowedFooters);

        await EnsureCurrentReportHeaderFooterLoadedAsync(reportDesignerModel, reportConfig);

        await headerFooterUsageService.PopulateLinkedReportCountsAsync(reportDesignerModel);

        // Populate data section definitions from all data sections in report categories
        reportDesignerModel.DataSectionsDefinition = await LoadDataSectionDefinitionsAsync(viewInfo?.ReportCategories);

        // Return the ReportDesignerModel
        logWriter.LogInfo($"Successfully retrieved report designer model for: {reportId.Name} with view: {viewInfo?.ViewName} and {sectionDefinitions.Count} section definitions", nameof(ReportDesignerHandler), nameof(GetReportDesignerConfiguration));
        return reportDesignerModel;
    }

    /// <summary>
    /// Finds the view that contains the specified report.
    /// </summary>
    /// <param name="reportName">The name of the report to find the view for.</param>
    /// <returns>View information including name and datasections, or null if not found.</returns>
    private async Task<ViewInfo> FindViewContainingReportAsync(string reportName)
    {
        try
        {
            var viewNames = new[] { "specimens", "cultures", "patients", "approvedreportview", "unapprovedreportview" };

            foreach (var viewName in viewNames)
            {
                var viewConfig = await listViewConfigFactory.GetViewAsync(viewName);
                if (viewConfig?.Reports != null && viewConfig.Reports.Contains(reportName.ToLower()))
                {
                    return new ViewInfo
                    {
                        ViewName = viewConfig.Name,
                        ReportCategories = viewConfig.ReportCategories,
                        AllowedHeaders = viewConfig.AllowedHeaders,
                        AllowedFooters = viewConfig.AllowedFooters
                    };
                }
            }

            logWriter.LogInfo($"No view found containing report: {reportName}", nameof(ReportDesignerHandler), nameof(FindViewContainingReportAsync));
            return null;
        }
        catch (Exception ex)
        {
            logWriter.LogError($"Error finding view for report {reportName}: {ex.Message}", nameof(ReportDesignerHandler), nameof(FindViewContainingReportAsync));
            return null;
        }
    }

    /// <summary>
    /// Loads section definitions for all sections included in the report categories.
    /// </summary>
    /// <remarks>
    /// Each section is resolved against the ConfigTypeId its category implies, so that a section name
    /// held by more than one configuration type is matched to the record this report actually uses.
    /// </remarks>
    /// <param name="categories">The list of report categories containing section names.</param>
    /// <returns>A list of ReportSectionModel objects representing the section definitions.</returns>
    private async Task<List<ReportSectionModel>> LoadSectionDefinitionsAsync(List<ReportCategoryModel> categories)
    {
        var sectionDefinitions = new List<ReportSectionModel>();
        var processedSections = new HashSet<string>(); // To avoid duplicates

        try
        {
            foreach (var category in categories)
            {
                var expectedConfigTypeId = SectionConfigTypeIdForCategory(category);

                if (category.Sections != null)
                {
                    foreach (var sectionName in category.Sections)
                    {
                        if (!string.IsNullOrEmpty(sectionName) && !processedSections.Contains(sectionName))
                        {
                            try
                            {
                                logWriter.LogInfo($"Loading section definition for: {sectionName}", nameof(ReportDesignerHandler), nameof(LoadSectionDefinitionsAsync));

                                // Get the section configuration using SectionAdapter
                                var sectionConfig = await sectionAdapter.GetSectionAsync(sectionName);

                                if (sectionConfig != null)
                                {
                                    // Map the section configuration to ReportSectionModel
                                    var sectionModel = sectionMapper.Map(sectionConfig);
                                    var configRecord = ResolveConfigRecord(sectionName, expectedConfigTypeId);
                                    sectionModel.ConfigId = configRecord.ConfigId;
                                    sectionModel.ConfigTypeId = configRecord.ConfigTypeId;
                                    sectionModel.Scope = ConfigScope.Section;
                                    sectionModel.State = ConfigChangeState.Unchanged;
                                    sectionDefinitions.Add(sectionModel);
                                    processedSections.Add(sectionName);

                                    logWriter.LogInfo($"Successfully loaded section definition for: {sectionName}", nameof(ReportDesignerHandler), nameof(LoadSectionDefinitionsAsync));
                                }
                                else
                                {
                                    logWriter.LogInfo($"No section configuration found for: {sectionName}", nameof(ReportDesignerHandler), nameof(LoadSectionDefinitionsAsync));
                                }
                            }
                            catch (System.Exception ex)
                            {
                                logWriter.LogError($"Error loading section definition for {sectionName}: {ex.Message}", nameof(ReportDesignerHandler), nameof(LoadSectionDefinitionsAsync));
                                // Continue with other sections even if one fails
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logWriter.LogError($"Error loading section definitions: {ex.Message}", nameof(ReportDesignerHandler), nameof(LoadSectionDefinitionsAsync));
        }

        return sectionDefinitions;
    }

    /// <summary>
    /// Populates AvailableFields for each reference by aggregating fields from the reference's
    /// whitelisted data sections. Fields are deduplicated by Value (stable field id); labels are
    /// display-only. Logs a warning when different field ids share the same label token.
    /// </summary>
    /// <param name="references">The list of report references.</param>
    private async Task PopulateAvailableFieldsAsync(List<ReportReferenceModel> references)
    {
        if (references == null || references.Count == 0)
        {
            return;
        }

        foreach (var reference in references)
        {
            try
            {
                var uniqueFields = new Dictionary<string, ReportAvailableFieldModel>(System.StringComparer.OrdinalIgnoreCase);
                var sectionCount = 0;

                if (reference.AvailableDataSections != null)
                {
                    foreach (var dataSectionName in reference.AvailableDataSections)
                    {
                        if (string.IsNullOrWhiteSpace(dataSectionName))
                        {
                            continue;
                        }

                        try
                        {
                            var dataSectionConfig = await dataSectionAdapter.GetSectionAsync(dataSectionName);
                            if (dataSectionConfig?.Fields != null)
                            {
                                sectionCount++;
                                foreach (var field in dataSectionConfig.Fields)
                                {
                                    if (!string.IsNullOrEmpty(field?.Value) && !uniqueFields.ContainsKey(field.Value))
                                    {
                                        uniqueFields[field.Value] = new ReportAvailableFieldModel
                                        {
                                            Name = field.Value,
                                            Label = field.Label
                                        };
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            logWriter.LogError($"Error loading data section fields for '{dataSectionName}': {ex.Message}", nameof(ReportDesignerHandler), nameof(PopulateAvailableFieldsAsync));
                        }
                    }
                }

                reference.AvailableFields = [.. uniqueFields.Values];

                logWriter.LogInfo(
                    $"Available fields for reference '{reference.Name}': {reference.AvailableFields.Count} fields from {sectionCount} data sections",
                    nameof(ReportDesignerHandler),
                    nameof(PopulateAvailableFieldsAsync));

                LogDuplicateFieldLabels(reference.Name, reference.AvailableFields);
            }
            catch (Exception ex)
            {
                logWriter.LogError($"Error aggregating available fields for reference '{reference?.Name}': {ex.Message}", nameof(ReportDesignerHandler), nameof(PopulateAvailableFieldsAsync));
            }
        }
    }

    /// <summary>
    /// Logs a warning for each label token shared by more than one field id within a reference.
    /// </summary>
    /// <param name="referenceName">The report reference name being aggregated.</param>
    /// <param name="fields">The deduplicated available fields for the reference.</param>
    private void LogDuplicateFieldLabels(string referenceName, List<ReportAvailableFieldModel> fields)
    {
        if (fields == null || fields.Count == 0)
        {
            return;
        }

        var labelGroups = fields
            .Where(f => !string.IsNullOrWhiteSpace(f?.Label) && !string.IsNullOrWhiteSpace(f?.Name))
            .GroupBy(f => f.Label, System.StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1);

        foreach (var group in labelGroups)
        {
            var fieldNames = string.Join(", ", group.Select(f => f.Name));
            logWriter.LogWarning(
                $"Duplicate label '{group.Key}' for fields {fieldNames} in reference '{referenceName}'",
                nameof(ReportDesignerHandler),
                nameof(PopulateAvailableFieldsAsync));
        }
    }

    /// <summary>
    /// Loads all available section formats from the config cache and returns custom format models,
    /// each carrying the configs identity it was loaded from so the designer can save by id rather than by name.
    /// </summary>
    /// <remarks>
    /// Formats live under <see cref="SectionFormatConfigTypeId"/>. Databases that have not yet run the
    /// migration splitting formats away from the shared settings type are still read from
    /// <see cref="LegacySectionFormatConfigTypeId"/>, and every legacy record found is logged.
    /// </remarks>
    /// <returns>The custom formats available to the designer.</returns>
    private Task<List<CustomFormatModel>> LoadCustomFormatsAsync()
    {
        var customFormats = new List<CustomFormatModel>();

        var formatConfigs = configCache.GetConfigList(SectionFormatConfigTypeId);
        var configTypeUsed = SectionFormatConfigTypeId;

        if (formatConfigs == null || formatConfigs.Count == 0)
        {
            formatConfigs = configCache.GetConfigList(LegacySectionFormatConfigTypeId);
            configTypeUsed = LegacySectionFormatConfigTypeId;
            logWriter.LogInfo(
                $"No section formats found on ConfigType {SectionFormatConfigTypeId}, falling back to the legacy ConfigType {LegacySectionFormatConfigTypeId}. Run migration 004-report-section-format-configtype.sql to complete the split.",
                nameof(ReportDesignerHandler), nameof(LoadCustomFormatsAsync));
        }

        foreach (var configRecord in formatConfigs ?? [])
        {
            try
            {
                if (configRecord.IsContentsNullOrEmptyJsonString())
                {
                    logWriter.LogInfo($"Section format '{configRecord.ConfigName}' has empty contents, skipping", nameof(ReportDesignerHandler), nameof(LoadCustomFormatsAsync));
                    continue;
                }

                var formatConfig = JsonConvert.DeserializeObject<ReportSectionFormatConfig>(configRecord.Contents);
                if (formatConfig != null)
                {
                    var customFormat = customFormatMapper.Map(formatConfig);
                    customFormat.ConfigId = configRecord.Id;
                    customFormat.State = ConfigChangeState.Unchanged;
                    customFormats.Add(customFormat);
                }
            }
            catch (Exception ex)
            {
                logWriter.LogError($"Error loading section format '{configRecord.ConfigName}' (id {configRecord.Id}, type {configTypeUsed}): {ex.Message}", nameof(ReportDesignerHandler), nameof(LoadCustomFormatsAsync));
            }
        }

        logWriter.LogInfo($"Loaded {customFormats.Count} section format(s) from ConfigType {configTypeUsed}", nameof(ReportDesignerHandler), nameof(LoadCustomFormatsAsync));

        return Task.FromResult(customFormats);
    }

    /// <summary>
    /// Looks up the configs identity and type for a configuration name so both can be sent to the designer.
    /// </summary>
    /// <remarks>
    /// The designer sends the identity and type back on save, which is what lets the record be matched
    /// exactly rather than by name alone. A record of the expected type is preferred, because a
    /// configuration name is only unique within a type: 'apipanelsection', for example, exists as both
    /// a main section and an organism section, and only the organism one belongs to a report that lists
    /// it under its organism category. The type is returned even when no record exists, so a section
    /// the report names but the database has not yet stored is still saved to the right type.
    /// </remarks>
    /// <param name="configName">The configuration name to resolve.</param>
    /// <param name="expectedConfigTypeId">The ConfigTypeId the caller expects the record to belong to.</param>
    /// <returns>The configs identity, null when the name is not in the cache, and the type to save under.</returns>
    private (int? ConfigId, int ConfigTypeId) ResolveConfigRecord(string configName, int expectedConfigTypeId)
    {
        if (string.IsNullOrWhiteSpace(configName))
        {
            return (null, expectedConfigTypeId);
        }

        var normalisedName = configName.NormalisedConfigName();

        var expectedTypeRecord = configCache
            .GetConfigList(expectedConfigTypeId)
            .FirstOrDefault(record => record.ConfigName.IsSameConfigName(normalisedName));

        if (expectedTypeRecord != null)
        {
            return (expectedTypeRecord.Id, expectedConfigTypeId);
        }

        var anyTypeRecord = configCache.GetConfig(normalisedName);

        if (anyTypeRecord == null)
        {
            return (null, expectedConfigTypeId);
        }

        logWriter.LogInfo(
            $"Configuration '{normalisedName}' was expected under ConfigType {expectedConfigTypeId} but was found under {anyTypeRecord.ConfigTypeId} (id {anyTypeRecord.Id}). Saves will go to the record that exists.",
            nameof(ReportDesignerHandler), nameof(ResolveConfigRecord));

        return (anyTypeRecord.Id, anyTypeRecord.ConfigTypeId);
    }

    /// <summary>
    /// Gives the ConfigTypeId that sections of a report category are stored under.
    /// </summary>
    /// <remarks>
    /// Organism sections have their own type; main and final sections share the other. This matches
    /// the split AddNewReportCommand applies when it creates a report's sections.
    /// </remarks>
    /// <param name="category">The report category.</param>
    /// <returns>The ConfigTypeId for sections in that category.</returns>
    private static int SectionConfigTypeIdForCategory(ReportCategoryModel category)
    {
        var isOrganism = category?.Source.IsSameConfigName("Organism") == true
                         || category?.SourceName.IsSameConfigName("OrganismSections") == true;

        return isOrganism ? ReportConfigTypes.OrganismSection : ReportConfigTypes.MainSection;
    }

    /// <summary>
    /// Loads data section definitions for all data sections specified in the report categories.
    /// </summary>
    /// <param name="reportCategories">List of report categories containing dataSections.</param>
    /// <returns>List of DataSectionDefinitionModel objects for all unique data sections.</returns>
    private async Task<List<DataSectionDefinitionModel>> LoadDataSectionDefinitionsAsync(List<ReportCategoryAvailabilityConfig> reportCategories)
    {
        var dataSectionDefinitions = new List<DataSectionDefinitionModel>();
        var processedDataSections = new HashSet<string>();

        if (reportCategories == null || !reportCategories.Any())
        {
            return dataSectionDefinitions;
        }

        // Collect all unique data section names from all report categories
        var allDataSectionNames = new List<string>();
        foreach (var category in reportCategories)
        {
            if (!string.IsNullOrWhiteSpace(category.DataSections))
            {
                var dataSectionNames = category.DataSections.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                          .Select(name => name.Trim())
                                                          .Where(name => !string.IsNullOrWhiteSpace(name));
                allDataSectionNames.AddRange(dataSectionNames);
            }
        }

        // Remove duplicates and process each unique data section
        var uniqueDataSectionNames = allDataSectionNames.Distinct().ToList();

        foreach (var dataSectionName in uniqueDataSectionNames)
        {
            if (processedDataSections.Contains(dataSectionName))
            {
                continue; // Skip if already processed
            }

            try
            {
                var dataSectionConfig = await dataSectionAdapter.GetSectionAsync(dataSectionName);
                if (dataSectionConfig != null)
                {
                    var gridModels = dataSectionConfig.Grids?.Select(g => new DataSectionGridModel
                    {
                        Name = g.Name,
                        Data = g.Data,
                        Description = g.Description
                    }).ToList() ?? new List<DataSectionGridModel>();

                    LogDataSectionGridDescriptions(dataSectionConfig.Name, gridModels);

                    var dataSectionDefinition = new DataSectionDefinitionModel
                    {
                        Name = dataSectionConfig.Name,
                        Title = dataSectionConfig.Title,
                        Fields = dataSectionConfig.Fields?.Select(f => new DataSectionFieldModel
                        {
                            Label = f.Label,
                            Value = f.Value
                        }).ToList() ?? new List<DataSectionFieldModel>(),
                        Grids = gridModels
                    };
                    dataSectionDefinitions.Add(dataSectionDefinition);
                    processedDataSections.Add(dataSectionName);
                }
            }
            catch (Exception ex)
            {
                logWriter.LogError($"Error loading data section '{dataSectionName}': {ex.Message}", nameof(ReportDesignerHandler), nameof(LoadDataSectionDefinitionsAsync));
            }
        }

        return dataSectionDefinitions;
    }

    /// <summary>
    /// Loads header section definitions for all headers listed in the allowedHeaders string.
    /// </summary>
    /// <param name="allowedHeaders">Comma-separated list of header names.</param>
    /// <returns>List of ReportSectionModel objects for all available headers.</returns>
    private async Task<List<ReportSectionModel>> LoadHeaderSectionDefinitionsAsync(string allowedHeaders)
    {
        var headerDefinitions = new List<ReportSectionModel>();

        if (string.IsNullOrWhiteSpace(allowedHeaders))
        {
            return headerDefinitions;
        }

        // Split the comma-separated list of header names
        var headerNames = allowedHeaders.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(name => name.Trim())
                                      .Where(name => !string.IsNullOrWhiteSpace(name));

        foreach (var headerName in headerNames)
        {
            var headerDefinition = await TryLoadHeaderSectionDefinitionAsync(headerName);
            if (headerDefinition != null)
            {
                headerDefinitions.Add(headerDefinition);
            }
        }

        return headerDefinitions;
    }

    private async Task<ReportSectionModel> TryLoadHeaderSectionDefinitionAsync(string headerName)
    {
        return await TryLoadHeaderOrFooterSectionAsync(
            headerName,
            "Header",
            () => reportHeaderAdapter.GetHeaderAsync(headerName));
    }

    /// <summary>
    /// Generic helper method to load header or footer section definitions.
    /// </summary>
    /// <remarks>
    /// Headers and footers often have no configs record at all, because the adapters fall back to the
    /// definitions held in code. The type is still stamped on the model so that the first time the
    /// designer saves an edit the record is created under the right type rather than as a report section.
    /// </remarks>
    /// <param name="sectionName">The name of the section to load.</param>
    /// <param name="sectionType">The type of section (Header or Footer).</param>
    /// <param name="getConfigFunc">Function to retrieve the configuration.</param>
    /// <returns>ReportSectionModel if successful, null otherwise.</returns>
    private async Task<ReportSectionModel> TryLoadHeaderOrFooterSectionAsync(string sectionName, string sectionType, Func<Task<ReportHeaderFooterConfig>> getConfigFunc)
    {
        if (string.IsNullOrWhiteSpace(sectionName))
        {
            return null;
        }

        try
        {
            var config = await getConfigFunc();
            if (config == null)
            {
                return null;
            }

            // Convert ReportHeaderFooterConfig to ReportSectionModel using the mapper
            var sectionModel = headerFooterMapper.Map((config, sectionType));
            if (sectionModel != null)
            {
                var expectedConfigTypeId = sectionType.IsSameConfigName(ConfigScope.Footer)
                    ? ReportConfigTypes.ReportFooter
                    : ReportConfigTypes.ReportHeader;

                var configRecord = ResolveConfigRecord(sectionName, expectedConfigTypeId);
                sectionModel.ConfigId = configRecord.ConfigId;
                sectionModel.ConfigTypeId = configRecord.ConfigTypeId;
                sectionModel.Scope = sectionType.IsSameConfigName(ConfigScope.Footer)
                    ? ConfigScope.Footer
                    : ConfigScope.Header;
                sectionModel.State = ConfigChangeState.Unchanged;
            }

            return sectionModel;
        }
        catch (Exception ex)
        {
            logWriter.LogError($"Error loading {sectionType.ToLower()} section '{sectionName}': {ex.Message}", nameof(ReportDesignerHandler), nameof(TryLoadHeaderOrFooterSectionAsync));
            return null;
        }
    }

    /// <summary>
    /// Loads footer section definitions for all footers listed in the allowedFooters string.
    /// </summary>
    /// <param name="allowedFooters">Comma-separated list of footer names.</param>
    /// <returns>List of ReportSectionModel objects for all available footers.</returns>
    private async Task<List<ReportSectionModel>> LoadFooterSectionDefinitionsAsync(string allowedFooters)
    {
        var footerDefinitions = new List<ReportSectionModel>();

        if (string.IsNullOrWhiteSpace(allowedFooters))
        {
            return footerDefinitions;
        }

        // Split the comma-separated list of footer names
        var footerNames = allowedFooters.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(name => name.Trim())
                                      .Where(name => !string.IsNullOrWhiteSpace(name));

        foreach (var footerName in footerNames)
        {
            var footerDefinition = await TryLoadFooterSectionDefinitionAsync(footerName);
            if (footerDefinition != null)
            {
                footerDefinitions.Add(footerDefinition);
            }
        }

        return footerDefinitions;
    }

    private async Task<ReportSectionModel> TryLoadFooterSectionDefinitionAsync(string footerName)
    {
        return await TryLoadHeaderOrFooterSectionAsync(
            footerName,
            "Footer",
            () => reportFooterAdapter.GetFooterAsync(footerName));
    }

    /// <summary>
    /// Ensures the report's selected header and footer definitions are loaded even when they are not
    /// listed in the view allowlist, which is the case for report-specific forked copies.
    /// </summary>
    /// <param name="model">The designer model being populated.</param>
    /// <param name="reportConfig">The report configuration being edited.</param>
    private async Task EnsureCurrentReportHeaderFooterLoadedAsync(ReportDesignerModel model, ReportConfig reportConfig)
    {
        if (model == null || reportConfig == null)
        {
            return;
        }

        model.HeaderSectionDefinitions ??= [];
        model.FooterSectionDefinitions ??= [];

        if (!string.IsNullOrWhiteSpace(reportConfig.Header)
            && !model.HeaderSectionDefinitions.Any(section => section.Name.IsSameConfigName(reportConfig.Header)))
        {
            var headerDefinition = await TryLoadHeaderSectionDefinitionAsync(reportConfig.Header);
            if (headerDefinition != null)
            {
                model.HeaderSectionDefinitions.Add(headerDefinition);
                logWriter.LogInfo(
                    $"Loaded report-specific header '{reportConfig.Header}' for report '{reportConfig.Name}' (not in AllowedHeaders).",
                    nameof(ReportDesignerHandler),
                    nameof(EnsureCurrentReportHeaderFooterLoadedAsync));
            }
        }

        if (!string.IsNullOrWhiteSpace(reportConfig.Footer)
            && !model.FooterSectionDefinitions.Any(section => section.Name.IsSameConfigName(reportConfig.Footer)))
        {
            var footerDefinition = await TryLoadFooterSectionDefinitionAsync(reportConfig.Footer);
            if (footerDefinition != null)
            {
                model.FooterSectionDefinitions.Add(footerDefinition);
                logWriter.LogInfo(
                    $"Loaded report-specific footer '{reportConfig.Footer}' for report '{reportConfig.Name}' (not in AllowedFooters).",
                    nameof(ReportDesignerHandler),
                    nameof(EnsureCurrentReportHeaderFooterLoadedAsync));
            }
        }
    }

    /// <summary>
    /// Parses the comma-separated datasections string into a list.
    /// </summary>
    /// <param name="dataSectionsString">The comma-separated string of datasections.</param>
    /// <returns>A list of datasection names.</returns>
    private static List<string> ParseDataSections(string dataSectionsString)
    {
        if (string.IsNullOrEmpty(dataSectionsString))
        {
            return new List<string>();
        }

        return dataSectionsString.Split(',')
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrEmpty(s))
            .ToList();
    }

    /// <summary>
    /// Gets the section list from report config based on the section source name.
    /// </summary>
    /// <param name="reportConfig">The report configuration.</param>
    /// <param name="sectionSourceName">The name of the section source (e.g., "MainSections", "OrganismSections").</param>
    /// <returns>The list of sections or null if not found.</returns>
    private static List<string> GetSectionListByName(ReportConfig reportConfig, string sectionSourceName)
    {
        return sectionSourceName.ToLower() switch
        {
            "mainsections" => reportConfig.MainSections,
            "organismsections" => reportConfig.OrganismSections,
            "finalsections" => reportConfig.FinalSections,
            _ => null
        };
    }

    /// <summary>
    /// Gets the display name for a category based on the section source name.
    /// Categories are always fixed: Main, Organism, and Final.
    /// </summary>
    /// <param name="sectionSourceName">The name of the section source (e.g., "MainSections", "OrganismSections", "FinalSections").</param>
    /// <returns>The display name for the category.</returns>
    private static string GetCategoryDisplayName(string sectionSourceName)
    {
        return sectionSourceName.ToLower() switch
        {
            "mainsections" => "Main Sections",
            "organismsections" => "Organism Sections",
            "finalsections" => "Final Sections",
            _ => "Main Sections" // Default fallback (should not occur with fixed categories)
        };
    }

    /// <summary>
    /// Resolves the category type based on the section source name.
    /// Categories are always fixed: Main, Organism, and Final.
    /// </summary>
    /// <param name="sectionSourceName">The name of the section source (e.g., "MainSections", "OrganismSections", "FinalSections").</param>
    /// <param name="sourceType">The source type (unused, kept for backward compatibility).</param>
    /// <returns>The category type.</returns>
    private static string ResolveCategoryType(string sectionSourceName, string sourceType)
    {
        return sectionSourceName.ToLower() switch
        {
            "mainsections" => "Main",
            "organismsections" => "Organism",
            "finalsections" => "Final",
            _ => "Main" // Default fallback (should not occur with fixed categories)
        };
    }

    /// <summary>
    /// Logs grid description coverage for a data section loaded into the report designer.
    /// </summary>
    /// <param name="dataSectionName">The data section configuration name.</param>
    /// <param name="grids">Grid models sent to the designer.</param>
    private void LogDataSectionGridDescriptions(string dataSectionName, List<DataSectionGridModel> grids)
    {
        if (grids == null || grids.Count == 0)
        {
            return;
        }

        var missingCount = 0;
        foreach (var grid in grids)
        {
            if (IsGridDescriptionMissing(grid.Description))
            {
                missingCount++;
                logWriter.LogWarning(
                    $"Data section '{dataSectionName}' grid '{grid.Name}' has no usable Description for the Grid Layout Editor dropdown",
                    nameof(ReportDesignerHandler),
                    nameof(LoadDataSectionDefinitionsAsync));
            }
        }

        var validCount = grids.Count - missingCount;
        logWriter.LogInfo(
            $"Data section '{dataSectionName}' grids: total={grids.Count}, withDescription={validCount}, missingDescription={missingCount}",
            nameof(ReportDesignerHandler),
            nameof(LoadDataSectionDefinitionsAsync));
    }

    private static bool IsGridDescriptionMissing(string description) =>
        string.IsNullOrWhiteSpace(description)
        || description.Equals("unknown", StringComparison.OrdinalIgnoreCase);
}
