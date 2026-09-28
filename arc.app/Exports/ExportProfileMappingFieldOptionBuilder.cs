using arc.app.Common;
using arc.app.Configuration;
using arc.common.Models;
using arc.common.Models.Export;
using arc.domain.Configuration.PagesConfig;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Exports
{
    /// <summary>
    /// Default implementation of <see cref="IExportProfileMappingFieldOptionBuilder"/>. Walks the
    /// profile field list and produces:
    ///   - one parent option per profile field (with <see cref="ExportProfileMappingFieldOption.IsGridField"/>
    ///     set when the form definition contains a matching grid),
    ///   - one flattened sub-field option per grid column, carrying
    ///     <see cref="ExportProfileMappingFieldOption.ParentGridKey"/> / <see cref="ExportProfileMappingFieldOption.GridSubFieldId"/>
    ///     so the editor can offer the column as a regular attribute outside the grid array, and the
    ///     export-time logic (future prompt) can resolve the column back to the parent grid.
    /// The same key shape is used by the editor and by the validator so that any flattened key the
    /// user can pick is automatically accepted on save.
    /// </summary>
    public class ExportProfileMappingFieldOptionBuilder : IExportProfileMappingFieldOptionBuilder
    {
        private readonly IFormConfigDefinition _formConfigDefinition;
        private readonly ILanguageHandler _languageHandler;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportProfileMappingFieldOptionBuilder"/> class.
        /// </summary>
        public ExportProfileMappingFieldOptionBuilder(
            IFormConfigDefinition formConfigDefinition,
            ILanguageHandler languageHandler,
            ILogWriter logWriter)
        {
            _formConfigDefinition = formConfigDefinition;
            _languageHandler = languageHandler;
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public async Task<List<ExportProfileMappingFieldOption>> BuildAsync(
            IReadOnlyCollection<ExportProfileFieldModel> fields,
            TokenInfoModel token)
        {
            var options = new List<ExportProfileMappingFieldOption>();
            if (fields == null) return options;

            foreach (var field in fields)
            {
                var parentLabel = await TranslateAsync(field.HeaderName ?? field.FieldName, token);
                var parent = new ExportProfileMappingFieldOption
                {
                    Key = BuildParentKey(field),
                    FieldId = field.FieldName,
                    FormName = field.FormName,
                    TableName = field.TableName,
                    Text = parentLabel,
                    Category = ClassifyField(field)
                };

                if (IsTestTable(field.TableName) && !string.IsNullOrWhiteSpace(field.FormName) &&
                    !string.Equals(field.FormName, "custom", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var formConfig = await _formConfigDefinition.LoadFormAsync(field.FormName);
                        var grid = formConfig.GetGridFieldList(field.FieldName) ?? new List<FieldGridConfig>();
                        if (grid.Count > 0)
                        {
                            parent.IsGridField = true;
                            foreach (var gridField in grid)
                            {
                                var subLabel = await TranslateAsync(gridField.Label ?? gridField.Id, token);
                                parent.GridSubFields.Add(new ExportProfileMappingGridSubField
                                {
                                    Id = gridField.Id,
                                    Label = subLabel,
                                    Type = gridField.Type
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logWriter.LogInfo(
                            $"WARN: Could not resolve grid sub-fields for {field.FormName}/{field.FieldName}: {ex.Message}",
                            nameof(ExportProfileMappingFieldOptionBuilder),
                            nameof(BuildAsync));
                    }
                }

                options.Add(parent);

                if (parent.IsGridField)
                {
                    foreach (var sub in parent.GridSubFields)
                    {
                        options.Add(new ExportProfileMappingFieldOption
                        {
                            Key = BuildFlattenedKey(field, sub.Id),
                            FieldId = field.FieldName,
                            FormName = field.FormName,
                            TableName = field.TableName,
                            Text = $"{sub.Label} ({parent.Text})",
                            Category = parent.Category,
                            IsGridField = false,
                            ParentGridKey = parent.Key,
                            GridSubFieldId = sub.Id
                        });
                    }
                }
            }

            return options;
        }

        /// <summary>
        /// Builds the canonical parent option key in the format
        /// <c>{fieldId}|{formName}|{tableName}|{header}</c>. Mirrors the legacy key shape used by the
        /// rest of the export pipeline.
        /// </summary>
        public static string BuildParentKey(ExportProfileFieldModel field)
            => $"{field.FieldName}|{field.FormName}|{field.TableName}|{field.HeaderName}";

        /// <summary>
        /// Builds the synthetic flattened-sub-field key. The <c>|sub:{id}</c> discriminator is appended
        /// so the new keys never collide with the legacy four-segment shape and are easy to recognise.
        /// </summary>
        public static string BuildFlattenedKey(ExportProfileFieldModel field, string subFieldId)
            => $"{BuildParentKey(field)}|sub:{subFieldId}";

        /// <summary>
        /// Classifies an export profile field into one of the categories used by the editor:
        /// patient, specimen, culture, culturetest, directtest, ast, or custom.
        /// </summary>
        public static string ClassifyField(ExportProfileFieldModel field)
        {
            if (field?.TableName == null) return "custom";
            return field.TableName.ToLowerInvariant() switch
            {
                "patient" => "patient",
                "culture" => "culture",
                "culturetests" => "culturetest",
                "tests" => "directtest",
                "ast" => "ast",
                "specimen" => "specimen",
                "custom" => "custom",
                _ => "specimen"
            };
        }

        /// <summary>
        /// Returns true when the field belongs to a test/culturetests table whose forms can carry grid fields.
        /// </summary>
        private static bool IsTestTable(string tableName)
        {
            return string.Equals(tableName, "tests", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(tableName, "culturetests", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Translates a label using the caller's language when one is available. When no token or
        /// language id is supplied (for example the server-side validation path, which only needs the
        /// option keys and never the translated text) the raw text is returned unchanged. This avoids
        /// resolving the language cache with a null key, which throws an <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="raw">The raw label or language tag to translate.</param>
        /// <param name="token">The current user's token, or null when translation is not required.</param>
        /// <returns>The translated label, or the raw text when no language id is available.</returns>
        private async Task<string> TranslateAsync(string raw, TokenInfoModel token)
        {
            if (string.IsNullOrWhiteSpace(raw)) return raw ?? string.Empty;
            if (string.IsNullOrWhiteSpace(token?.LanguageId)) return raw;
            return await _languageHandler.TranslateAsync(raw, token.LanguageId);
        }
    }
}
