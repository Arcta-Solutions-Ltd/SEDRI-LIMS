using arc.app.Config.Forms;
using arc.app.Config.Queries;
using arc.app.Configuration;
using arc.app.Common;
using arc.app.Exports.Formatters;
using arc.app.Files;
using arc.common.Models;
using arc.common.Models.Export;
using arc.common.Utils;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    /// <summary>
    /// Handles export execution: runs the export query, stores the result as a file attachment,
    /// and creates an export history record with the criteria used.
    /// </summary>
    public class ExportRunHandler : IExportRunHandler
    {
        private readonly IExportRepository _exportRepository;
        private readonly IExportProfileFieldRepository _exportProfileFieldRepository;
        private readonly IFormConfigDefinition _formConfigDefinition;
        private readonly IQueryAdapter _queryAdapter;
        private readonly IExportPostQueryProcessor _exportPostQueryProcessor;
        private readonly IExportRunHistoryRepository _exportRunHistoryRepository;
        private readonly IExportScheduleRepository _exportScheduleRepository;
        private readonly IExportProfileMappingRepository _exportProfileMappingRepository;
        private readonly IExportFormatWriterFactory _exportFormatWriterFactory;
        private readonly IFileHandler _fileHandler;
        private readonly ILogWriter _logWriter;

        public ExportRunHandler(IExportRepository exportRepository, IExportProfileFieldRepository exportProfileFieldRepository, IQueryAdapter queryAdapter,
            IFormConfigDefinition formConfigDefinition, IExportPostQueryProcessor exportPostQueryProcessor,
            IExportRunHistoryRepository exportRunHistoryRepository, IExportScheduleRepository exportScheduleRepository,
            IExportProfileMappingRepository exportProfileMappingRepository, IExportFormatWriterFactory exportFormatWriterFactory,
            IFileHandler fileHandler, ILogWriter logWriter)
        {
            _exportRepository = exportRepository;
            _exportProfileFieldRepository = exportProfileFieldRepository;
            _queryAdapter = queryAdapter;
            _formConfigDefinition = formConfigDefinition;
            _exportPostQueryProcessor = exportPostQueryProcessor;
            _exportRunHistoryRepository = exportRunHistoryRepository;
            _exportScheduleRepository = exportScheduleRepository;
            _exportProfileMappingRepository = exportProfileMappingRepository;
            _exportFormatWriterFactory = exportFormatWriterFactory;
            _fileHandler = fileHandler;
            _logWriter = logWriter;
        }

        public async Task<string> RunExportAsync(QueryFilterConfig queryFilters, TokenInfoModel token)
        {
            await ApplyIncrementalScheduleContextAsync(queryFilters);

            _logWriter.LogInfo(
                $"Export run filters: exportProfileId={queryFilters.GetStringValue("exportprofileid")}, " +
                $"organisationIds={queryFilters.GetStringValue("organisationfilterid") ?? queryFilters.GetStringValue("organisationFilterId")}, " +
                $"locationIds={queryFilters.GetStringValue("locationid") ?? queryFilters.GetStringValue("locationId")}, " +
                $"tagIds={queryFilters.GetStringValue("tagid") ?? queryFilters.GetStringValue("tagId")}",
                nameof(ExportRunHandler),
                nameof(RunExportAsync));

            // Get the profile

            var exportProfileFields = (await _exportProfileFieldRepository.GetByProfileIdAsync(queryFilters)).ToList();
            if (exportProfileFields.Count == 0)
            {
                return "";
            }

            var profileId = queryFilters.GetStringValue("exportprofileid");

            // Resolve the profile's structural mapping (if any). The mapping format drives the output
            // format; when no mapping exists the export stays CSV.
            var mapping = await LoadMappingAsync(queryFilters, profileId);
            var format = string.IsNullOrWhiteSpace(mapping?.Format) ? "csv" : mapping.Format.Trim().ToLowerInvariant();
            _logWriter.LogInfo(
                mapping == null
                    ? $"No export mapping for profile {profileId}; using csv format"
                    : $"Export mapping found for profile {profileId}; using {format} format",
                nameof(ExportRunHandler), nameof(RunExportAsync));

            // Get specimen and patient view query definitions

            var specimenQuery = await _queryAdapter.GetQueryAsync("specimenforspecimenview");
            var patientQuery = await _queryAdapter.GetQueryAsync("patientforpatientview");
            var cultureQuery = await _queryAdapter.GetQueryAsync("cultureforcultureview");

            // Extract the list items from the specimen query definition

            var listItems = (specimenQuery.ListItems + "," + patientQuery.ListItems + "," + cultureQuery.ListItems).ToLower().Split(",");
            var multiSelectItems = (specimenQuery.MultiSelectItems + "," + patientQuery.MultiSelectItems).ToLower().Split(",");

            var fieldConfigurations = new List<FieldConfig>();

            foreach(var profile in exportProfileFields)
            {
                var newFieldEntry = new FieldConfig { Id = FieldNameTranslator(profile.FieldName), Type = "normal", TableName = profile.TableName,
                    Label = profile.FormName, MoreData = profile.MoreData
                };

                if (profile.TableName.ToLower() == "tests" || profile.TableName.ToLower() == "culturetests")
                {
                    var testFormConfig = await _formConfigDefinition.LoadFormAsync(profile.FormName);
                    var listFieldsOnForm = testFormConfig.GetListFieldsForForm();
                    newFieldEntry.GridFields = testFormConfig.GetGridFieldList(profile.FieldName);
                    var matchingListField = listFieldsOnForm.FirstOrDefault(f => f.Id.ToLower() == profile.FieldName.ToLower());
                    if (matchingListField != null)
                    {
                        newFieldEntry.Type = "list";
                        newFieldEntry.MultiSelect = matchingListField.MultiSelect;
                    }
                    var commentFields = testFormConfig.GetCommentFieldsForForm();
                    var matchingCommentField = commentFields.FirstOrDefault(f => f.Id.ToLower() == profile.FieldName.ToLower());
                    if(matchingCommentField != null)
                    {
                        newFieldEntry.IsComment = matchingCommentField.IsComment;
                    }
                }
                else
                {
                    var newEntry = newFieldEntry.Id.ToLower().EndsWith("id") ? newFieldEntry.Id[..^2].ToLower() : newFieldEntry.Id.ToLower();
                    var listEntry = listItems.Any(f => f.Trim() == newEntry.Trim());
                    var multiSelect = multiSelectItems.Any(f => f.Trim() == newEntry.Trim());
                    if (listEntry)
                    {
                        newFieldEntry.Type = "list";
                        newFieldEntry.Id = newEntry.Trim();
                        newFieldEntry.MultiSelect = multiSelect;
                    }
                    if (profile.FormName.ToLower() != "custom")
                    {
                        var formConfig = await _formConfigDefinition.LoadFormAsync(profile.FormName);
                        var commentFields = formConfig.GetCommentFieldsForForm();
                        var matchingCommentField = commentFields.FirstOrDefault(f => f.Id.ToLower() == profile.FieldName.ToLower());
                        if (matchingCommentField != null)
                        {
                            newFieldEntry.IsComment = matchingCommentField.IsComment;
                        }
                    }
                }

                fieldConfigurations.Add(newFieldEntry);
            }

            // When the mapping nests rows into arrays, append hidden id columns so the JSON/XML
            // writers can group rows by stable ids rather than by display values.
            if (mapping != null)
            {
                AppendHiddenGroupingColumns(mapping, exportProfileFields, fieldConfigurations);
            }

            // Create the header line

            var header = "#";
            foreach (var profile in exportProfileFields)
            {
                header = header == "#" ? profile.HeaderName : header + "|" + profile.HeaderName;
            }

            var lines = new List<string> { header };

            // Run the query

            var dataLines = await _exportRepository.ExportRunAsync(queryFilters, token, fieldConfigurations);
            lines.AddRange(dataLines);

            var processResult = await _exportPostQueryProcessor.Process(lines, exportProfileFields, fieldConfigurations, queryFilters, token);

            // Store export file and create history record
            var outputDirectory = queryFilters.GetStringValue("outputDirectory");
            var writer = _exportFormatWriterFactory.Create(format);
            var formatContext = new ExportFormatContext
            {
                Lines = processResult.Lines,
                ColumnKeys = processResult.ColumnKeys,
                Fields = exportProfileFields,
                Mapping = mapping,
                ProfileId = profileId
            };
            var fileContent = writer.Write(formatContext);
            int? fileAttachmentId = null;
            try
            {
                var filename = $"export_{profileId}_{DateTime.UtcNow:yyyyMMddHHmmss}.{writer.FileExtension}";
                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
                var uploadResult = await _fileHandler.UploadAsync(stream, filename, writer.ContentType, "export", string.IsNullOrWhiteSpace(outputDirectory) ? null : outputDirectory.Trim());
                fileAttachmentId = uploadResult.Id;
                _logWriter.LogInfo($"Export file uploaded successfully, fileAttachmentId={fileAttachmentId}, format={format}", nameof(ExportRunHandler), nameof(RunExportAsync));
            }
            catch (Exception ex)
            {
                _logWriter.LogError($"Failed to upload export file: {ex.Message}", nameof(ExportRunHandler), nameof(RunExportAsync));
            }

            try
            {
                var exportRunRequest = BuildExportRunRequestModel(queryFilters);
                exportRunRequest.FileAttachmentId = fileAttachmentId;
                if (queryFilters.TryParseIntegerValue("exportScheduleId", out var scheduleId) && scheduleId > 0)
                {
                    exportRunRequest.ExportScheduleId = scheduleId;
                }
                var historyId = await _exportRunHistoryRepository.AddExportRunAsync(exportRunRequest);
                _logWriter.LogInfo($"Export history record created, id={historyId}", nameof(ExportRunHandler), nameof(RunExportAsync));
            }
            catch (Exception ex)
            {
                _logWriter.LogError($"Failed to create export history record: {ex.Message}", nameof(ExportRunHandler), nameof(RunExportAsync));
            }

            // Return an envelope describing the output so the client can download it in the correct format.
            var result = new ExportRunResultModel
            {
                Format = format,
                FileName = $"export_run.{writer.FileExtension}"
            };
            if (format == "csv")
            {
                result.Rows = processResult.Lines;
            }
            else
            {
                result.Content = fileContent;
            }
            return ArcJson.Serialize(result);
        }

        /// <summary>
        /// Loads the structural mapping for the export profile, returning null when none is saved or the
        /// lookup fails (in which case the export falls back to CSV).
        /// </summary>
        /// <param name="queryFilters">The run's query filters (must carry the export profile id).</param>
        /// <param name="profileId">The export profile id (for diagnostics).</param>
        /// <returns>The persisted mapping, or null.</returns>
        private async Task<ExportProfileMappingModel> LoadMappingAsync(QueryFilterConfig queryFilters, string profileId)
        {
            try
            {
                return await _exportProfileMappingRepository.GetByProfileIdAsync(queryFilters);
            }
            catch (Exception ex)
            {
                _logWriter.LogError($"Failed to load export mapping for profile {profileId}: {ex.Message}", nameof(ExportRunHandler), nameof(LoadMappingAsync));
                return null;
            }
        }

        /// <summary>
        /// Appends hidden grouping-id columns (specimen / culture / ast id) to both the profile field
        /// list and the field configurations when the mapping tree contains arrays that need id-based
        /// grouping. The columns are matched later by their sentinel keys and never appear in CSV output.
        /// </summary>
        /// <param name="mapping">The profile's structural mapping.</param>
        /// <param name="exportProfileFields">The ordered profile fields (appended in place).</param>
        /// <param name="fieldConfigurations">The aligned field configurations (appended in place).</param>
        private void AppendHiddenGroupingColumns(ExportProfileMappingModel mapping, List<ExportProfileFieldModel> exportProfileFields, List<FieldConfig> fieldConfigurations)
        {
            var arrayTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            CollectArrayTypes(ParseStructure(mapping.Structure), arrayTypes);
            if (arrayTypes.Count == 0)
            {
                return;
            }

            var containsAstFields = fieldConfigurations.Any(r => (r.TableName ?? string.Empty).ToLower() == "ast");
            var containsCultureFields = containsAstFields || fieldConfigurations.Any(r =>
            {
                var t = (r.TableName ?? string.Empty).ToLower();
                return t == "culture" || t == "culturetests";
            });

            var appended = new List<string>();

            // Specimen id is always available (the export query is always rooted on "specimen s").
            if (arrayTypes.Contains("specimens"))
            {
                AppendHiddenColumn(exportProfileFields, fieldConfigurations, "__specimenid", "specimen");
                appended.Add("specimenid");
            }
            if (arrayTypes.Contains("cultures") && containsCultureFields)
            {
                AppendHiddenColumn(exportProfileFields, fieldConfigurations, "__cultureid", "culture");
                appended.Add("cultureid");
            }
            if (arrayTypes.Contains("ast") && containsAstFields)
            {
                AppendHiddenColumn(exportProfileFields, fieldConfigurations, "__astid", "ast");
                appended.Add("astid");
            }

            _logWriter.LogInfo(
                $"Appended hidden grouping columns for mapped export: [{string.Join(", ", appended)}] (array types: [{string.Join(", ", arrayTypes)}])",
                nameof(ExportRunHandler), nameof(AppendHiddenGroupingColumns));
        }

        /// <summary>
        /// Appends a single hidden id column definition to both aligned lists. The profile field carries
        /// the sentinel field name/header while the field config selects the raw id column via the
        /// table's SQL alias.
        /// </summary>
        private static void AppendHiddenColumn(List<ExportProfileFieldModel> exportProfileFields, List<FieldConfig> fieldConfigurations, string sentinel, string tableName)
        {
            exportProfileFields.Add(new ExportProfileFieldModel
            {
                FieldName = sentinel,
                HeaderName = sentinel,
                TableName = tableName,
                FormName = "Custom"
            });
            fieldConfigurations.Add(new FieldConfig
            {
                Id = "id",
                Type = "normal",
                TableName = tableName,
                Label = "Custom"
            });
        }

        /// <summary>
        /// Deserialises the mapping structure JSON, returning null when it is empty or invalid.
        /// </summary>
        private MappingStructureNode ParseStructure(string structureJson)
        {
            if (string.IsNullOrWhiteSpace(structureJson))
            {
                return null;
            }
            try
            {
                return ArcJson.Deserialize<MappingStructureNode>(structureJson);
            }
            catch (Exception ex)
            {
                _logWriter.LogError($"Failed to deserialise export mapping structure: {ex.Message}", nameof(ExportRunHandler), nameof(ParseStructure));
                return null;
            }
        }

        /// <summary>
        /// Recursively collects the array types present in the mapping tree so the handler knows which
        /// hidden grouping-id columns to append.
        /// </summary>
        private static void CollectArrayTypes(MappingStructureNode node, HashSet<string> arrayTypes)
        {
            if (node == null)
            {
                return;
            }
            if (string.Equals(node.Kind, "array", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(node.ArrayType))
            {
                arrayTypes.Add(node.ArrayType.Trim().ToLowerInvariant());
            }
            foreach (var child in node.Children ?? new List<MappingStructureNode>())
            {
                CollectArrayTypes(child, arrayTypes);
            }
        }

        /// <summary>
        /// When query filters include exportScheduleId and changesToInclude (or incrementalOnly), resolves the last run
        /// for that schedule and sets startdate, enddate, and datefieldname for incremental export.
        /// </summary>
        private async Task ApplyIncrementalScheduleContextAsync(QueryFilterConfig queryFilters)
        {
            if (!queryFilters.TryParseIntegerValue("exportScheduleId", out var scheduleId) || scheduleId <= 0)
            {
                return;
            }
            var changesToInclude = queryFilters.GetStringValue("changesToInclude");
            var incrementalOnly = string.Equals(queryFilters.GetStringValue("incrementalOnly"), "Yes", StringComparison.OrdinalIgnoreCase);
            var isIncremental = !string.IsNullOrWhiteSpace(changesToInclude) || incrementalOnly;
            if (!isIncremental)
            {
                return;
            }

            var lastRun = await _exportScheduleRepository.GetLastRunForScheduleAsync(scheduleId);
            if (lastRun.HasValue)
            {
                queryFilters.AddString("startdate", lastRun.Value.ToString("yyyy-MM-dd"));
                queryFilters.AddString("startDate", lastRun.Value.ToString("yyyy-MM-dd"));
            }
            queryFilters.AddString("enddate", DateTime.UtcNow.ToString("yyyy-MM-dd"));
            queryFilters.AddString("endDate", DateTime.UtcNow.ToString("yyyy-MM-dd"));
            var dateField = string.Equals(changesToInclude, "newonly", StringComparison.OrdinalIgnoreCase)
                ? "s.lastmodifieddate"
                : "s.lastmodifieddate";
            queryFilters.AddString("datefieldname", dateField);
            _logWriter.LogInfo($"Incremental export: scheduleId={scheduleId}, startDate={lastRun}, endDate=now, dateField={dateField}, changesToInclude={changesToInclude}", nameof(ExportRunHandler), nameof(ApplyIncrementalScheduleContextAsync));
        }

        /// <summary>
        /// Maps query filter parameters to ExportRunRequestModel for history storage.
        /// </summary>
        private static ExportRunRequestModel BuildExportRunRequestModel(QueryFilterConfig queryFilters)
        {
            static List<string> SplitIds(string value)
            {
                if (string.IsNullOrWhiteSpace(value)) return new List<string>();
                return value.Split(',', ';').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
            }

            return new ExportRunRequestModel
            {
                ExportProfileId = queryFilters.GetStringValue("exportprofileid") ?? "",
                StartDate = DateTime.TryParse(queryFilters.GetStringValue("startdate") ?? queryFilters.GetStringValue("startDate"), out var sd) ? sd : default,
                EndDate = DateTime.TryParse(queryFilters.GetStringValue("enddate") ?? queryFilters.GetStringValue("endDate"), out var ed) ? ed : default,
                SpecimenTypeIds = SplitIds(queryFilters.GetStringValue("specimentypeid") ?? queryFilters.GetStringValue("specimenTypeId") ?? ""),
                SpecimenStateIds = SplitIds(queryFilters.GetStringValue("stateid") ?? queryFilters.GetStringValue("stateId") ?? ""),
                TagIds = SplitIds(queryFilters.GetStringValue("tagid") ?? queryFilters.GetStringValue("tagId") ?? ""),
                OrganisationIds = SplitIds(queryFilters.GetStringValue("organisationfilterid") ?? queryFilters.GetStringValue("organisationFilterId") ?? ""),
                LocationIds = SplitIds(queryFilters.GetStringValue("locationid") ?? queryFilters.GetStringValue("locationId") ?? ""),
                TestIds = SplitIds(queryFilters.GetStringValue("testid") ?? queryFilters.GetStringValue("testId") ?? ""),
                OrganismIds = SplitIds(queryFilters.GetStringValue("organismid") ?? queryFilters.GetStringValue("organismId") ?? ""),
                ASTExclusive = queryFilters.GetStringValue("astexclusive") ?? queryFilters.GetStringValue("astExclusive")
            };
        }

        private string FieldNameTranslator(string fieldName)
        {
            return fieldName.ToLower() switch
            {
                "locationhierarchy" => "fullyqualifiedname",
                "organisationhierarchy" => "fullorganisationname",
                "tempinlast24hrsid" => "tempinlast24hrs",
                "laboratoryid" => "laboratoryname",
                "culturetype" => "typeid",
                "quantity" => "specimenquantity",
                "whonetantibiotic" => "id",
                "manufacturersbarcode" => "existingbarcode",
                "specimencomments" => "id",
                "culturecomments" => "id",
                "aliquotid" => "aloquatid",
                "qualitativeantibioticsusceptibility" => "id",
                "antibioticmeasurement" => "id",
                _ => fieldName,
            };
        }
    }
}
