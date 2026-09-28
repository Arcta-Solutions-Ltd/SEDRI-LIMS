using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using arc.app.Common;
using arc.app.Exports;
using arc.common.Models.Export;
using arc.common.Models.Instruments.CustomImport;
using arc.common.Utils;
using Newtonsoft.Json.Linq;

namespace arc.app.Instruments.CustomImport
{
    /// <summary>
    /// Default <see cref="ICustomImportRecordParser"/>. Walks the export profile mapping tree together with the parsed
    /// document (JSON or XML) and produces an <see cref="ImportRecord"/>. Attribute leaves are bound to their export
    /// profile field (by field id, never by translated header) so the source table/field is known. Nesting is driven by
    /// the mapping array types (<c>specimens</c> / <c>cultures</c> / <c>ast</c>), mirroring the export builder.
    /// </summary>
    public class CustomImportRecordParser : ICustomImportRecordParser
    {
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomImportRecordParser"/> class.
        /// </summary>
        /// <param name="logWriter">Log writer for installed-system diagnostics.</param>
        public CustomImportRecordParser(ILogWriter logWriter)
        {
            _logWriter = logWriter;
        }

        /// <summary>Holds the field lookup and array-type presence flags for a parse pass.</summary>
        private sealed class ParseContext
        {
            public IReadOnlyDictionary<string, ExportProfileFieldModel> FieldsById { get; init; }
            public bool HasSpecimensArray { get; init; }
            public bool HasCulturesArray { get; init; }
            public bool HasAstArray { get; init; }
        }

        /// <inheritdoc />
        public ImportRecord Parse(string fileContent, ExportProfileMappingModel mapping, IEnumerable<ExportProfileFieldModel> fields)
        {
            var record = new ImportRecord();
            var fieldList = (fields ?? Enumerable.Empty<ExportProfileFieldModel>()).ToList();
            record.HasPatientData = fieldList.Any(f =>
                string.Equals(UniqueReferenceTableBucket.Normalize(f.TableName), "patient", StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(fileContent) || mapping == null || string.IsNullOrWhiteSpace(mapping.Structure))
            {
                _logWriter.LogInfo("Custom import: empty file content or mapping; nothing parsed", nameof(CustomImportRecordParser), nameof(Parse));
                return record;
            }

            MappingStructureNode root;
            try
            {
                root = ArcJson.Deserialize<MappingStructureNode>(mapping.Structure);
            }
            catch (Exception ex)
            {
                throw new FormatException($"Export mapping structure could not be parsed: {ex.Message}");
            }

            var rootDoc = ParseDocument(fileContent, mapping.Format);
            if (rootDoc == null || root == null)
            {
                throw new FormatException("Inbound file could not be parsed against the export profile structure.");
            }

            var ctx = new ParseContext
            {
                FieldsById = fieldList.GroupBy(f => f.Id.ToString())
                    .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase),
                HasSpecimensArray = ContainsArrayType(root, "specimens"),
                HasCulturesArray = ContainsArrayType(root, "cultures"),
                HasAstArray = ContainsArrayType(root, "ast")
            };

            var hasSpecimenFields = fieldList.Any(f => IsBucket(f, "specimen"));
            var hasCultureFields = fieldList.Any(f => IsBucket(f, "culture"));

            // Pre-create root-level context entities when the mapping has no array for that level, so root-level
            // attributes have a home. Array items create their own entities as they are walked.
            var rootSpecimen = ctx.HasSpecimensArray ? null : EnsureSpecimen(record, null);
            var rootCulture = !ctx.HasCulturesArray && rootSpecimen != null && hasCultureFields
                ? EnsureCulture(rootSpecimen, null)
                : null;

            foreach (var child in root.Children ?? new List<MappingStructureNode>())
            {
                BuildChild(child, rootDoc, ctx, record, rootSpecimen, rootCulture, null);
            }

            // Drop an empty pre-created specimen (no fields, no cultures) to avoid creating blank rows.
            record.Specimens.RemoveAll(s => s.Fields.Count == 0 && s.Children.Count == 0);

            _logWriter.LogInfo(
                $"Custom import parsed: patient={(record.Patient != null ? "yes" : "no")}, specimens={record.Specimens.Count}",
                nameof(CustomImportRecordParser), nameof(Parse));
            return record;
        }

        private void BuildChild(MappingStructureNode node, IImportDocument doc, ParseContext ctx, ImportRecord record,
            ImportEntity specimen, ImportEntity culture, ImportEntity ast)
        {
            if (node == null)
            {
                return;
            }

            switch ((node.Kind ?? string.Empty).ToLowerInvariant())
            {
                case "attribute":
                    AssignAttribute(node, doc, ctx, record, specimen, culture, ast);
                    break;
                case "object":
                    var childDoc = doc.GetChildObject(node.Name) ?? doc;
                    foreach (var c in node.Children ?? new List<MappingStructureNode>())
                    {
                        BuildChild(c, childDoc, ctx, record, specimen, culture, ast);
                    }
                    break;
                case "array":
                    BuildArray(node, doc, ctx, record, specimen, culture);
                    break;
            }
        }

        private void BuildArray(MappingStructureNode node, IImportDocument doc, ParseContext ctx, ImportRecord record,
            ImportEntity specimen, ImportEntity culture)
        {
            var arrayType = (node.ArrayType ?? string.Empty).ToLowerInvariant();
            var items = doc.GetArrayItems(node.Name).ToList();

            switch (arrayType)
            {
                case "specimens":
                    foreach (var item in items)
                    {
                        var newSpecimen = EnsureSpecimen(record, null);
                        foreach (var c in node.Children ?? new List<MappingStructureNode>())
                        {
                            BuildChild(c, item, ctx, record, newSpecimen, null, null);
                        }
                    }
                    break;
                case "cultures":
                    foreach (var item in items)
                    {
                        var parentSpecimen = specimen ?? EnsureSpecimen(record, null);
                        var newCulture = EnsureCulture(parentSpecimen, null);
                        foreach (var c in node.Children ?? new List<MappingStructureNode>())
                        {
                            BuildChild(c, item, ctx, record, parentSpecimen, newCulture, null);
                        }
                    }
                    break;
                case "ast":
                    foreach (var item in items)
                    {
                        if (culture == null)
                        {
                            _logWriter.LogInfo("Custom import: ast array outside a culture context ignored", nameof(CustomImportRecordParser), nameof(BuildArray));
                            continue;
                        }
                        var newAst = new ImportEntity { EntityType = ImportEntityType.Ast };
                        culture.Children.Add(newAst);
                        foreach (var c in node.Children ?? new List<MappingStructureNode>())
                        {
                            BuildChild(c, item, ctx, record, specimen, culture, newAst);
                        }
                    }
                    break;
                default:
                    _logWriter.LogInfo($"Custom import: array type '{arrayType}' not supported for load; ignored", nameof(CustomImportRecordParser), nameof(BuildArray));
                    break;
            }
        }

        private void AssignAttribute(MappingStructureNode node, IImportDocument doc, ParseContext ctx, ImportRecord record,
            ImportEntity specimen, ImportEntity culture, ImportEntity ast)
        {
            if (string.IsNullOrWhiteSpace(node.FieldKey))
            {
                return; // unmapped/ignored attribute
            }

            var fieldId = FieldIdFromKey(node.FieldKey);
            if (!ctx.FieldsById.TryGetValue(fieldId, out var field))
            {
                return;
            }

            var value = doc.GetAttributeValue(node.Name);
            var bucket = UniqueReferenceTableBucket.Normalize(field.TableName);
            var fieldValue = new ImportFieldValue
            {
                FieldId = field.Id,
                FieldName = field.FieldName,
                FormName = field.FormName,
                TableName = field.TableName,
                Bucket = bucket,
                OutputName = node.Name,
                Value = value,
                IsUniqueReference = string.Equals(node.UniqueReference?.Trim(), "Yes", StringComparison.OrdinalIgnoreCase)
            };

            switch (bucket)
            {
                case "patient":
                    EnsurePatient(record).Fields.Add(fieldValue);
                    break;
                case "specimen":
                    (specimen ?? EnsureSpecimen(record, null)).Fields.Add(fieldValue);
                    break;
                case "culture":
                    if (culture != null) { culture.Fields.Add(fieldValue); }
                    break;
                case "ast":
                    if (ast != null) { ast.Fields.Add(fieldValue); }
                    break;
            }
        }

        private static ImportEntity EnsurePatient(ImportRecord record)
        {
            return record.Patient ??= new ImportEntity { EntityType = ImportEntityType.Patient };
        }

        private static ImportEntity EnsureSpecimen(ImportRecord record, ImportEntity current)
        {
            if (current != null)
            {
                return current;
            }
            var specimen = new ImportEntity { EntityType = ImportEntityType.Specimen };
            record.Specimens.Add(specimen);
            return specimen;
        }

        private static ImportEntity EnsureCulture(ImportEntity specimen, ImportEntity current)
        {
            if (current != null)
            {
                return current;
            }
            var culture = new ImportEntity { EntityType = ImportEntityType.Culture };
            specimen.Children.Add(culture);
            return culture;
        }

        private IImportDocument ParseDocument(string content, string format)
        {
            var normalizedFormat = string.IsNullOrWhiteSpace(format) ? "json" : format.Trim().ToLowerInvariant();
            try
            {
                if (normalizedFormat == "xml")
                {
                    var doc = XDocument.Parse(content);
                    return new XmlImportDocument(doc.Root);
                }

                var token = JToken.Parse(content);
                if (token is JArray array)
                {
                    var first = array.OfType<JObject>().FirstOrDefault();
                    return first == null ? null : new JsonImportDocument(first);
                }
                return token is JObject obj ? new JsonImportDocument(obj) : null;
            }
            catch (Exception ex)
            {
                _logWriter.LogError($"Custom import: failed to parse {normalizedFormat} document: {ex.Message}",
                    nameof(CustomImportRecordParser), nameof(ParseDocument));
                return null;
            }
        }

        private static bool IsBucket(ExportProfileFieldModel field, string bucket) =>
            string.Equals(UniqueReferenceTableBucket.Normalize(field.TableName), bucket, StringComparison.OrdinalIgnoreCase);

        private static bool ContainsArrayType(MappingStructureNode node, string arrayType)
        {
            if (node == null)
            {
                return false;
            }
            if (string.Equals(node.Kind, "array", StringComparison.OrdinalIgnoreCase)
                && string.Equals(node.ArrayType?.Trim(), arrayType, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return (node.Children ?? new List<MappingStructureNode>()).Any(c => ContainsArrayType(c, arrayType));
        }

        private static string FieldIdFromKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var pipe = key.IndexOf('|');
            return (pipe < 0 ? key : key.Substring(0, pipe)).Trim();
        }
    }
}
