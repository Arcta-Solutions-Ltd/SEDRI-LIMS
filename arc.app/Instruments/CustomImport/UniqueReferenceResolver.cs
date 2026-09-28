using System;
using System.Collections.Generic;
using System.Linq;
using arc.app.Common;
using arc.app.Exports;
using arc.common.Models.Export;
using arc.common.Utils;

namespace arc.app.Instruments.CustomImport
{
    /// <summary>
    /// Default <see cref="IUniqueReferenceResolver"/>. Walks the mapping tree for attributes flagged
    /// <c>uniqueReference: "Yes"</c>, resolves each to its export profile field (by field id) and records the field as the
    /// bucket's unique reference. Bucket defaults are applied when no attribute is flagged.
    /// </summary>
    public class UniqueReferenceResolver : IUniqueReferenceResolver
    {
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueReferenceResolver"/> class.
        /// </summary>
        /// <param name="logWriter">Log writer for installed-system diagnostics.</param>
        public UniqueReferenceResolver(ILogWriter logWriter)
        {
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public UniqueReferenceMap Resolve(string structureJson, IEnumerable<ExportProfileFieldModel> fields)
        {
            var map = new UniqueReferenceMap();
            var fieldsById = (fields ?? Enumerable.Empty<ExportProfileFieldModel>())
                .GroupBy(f => f.Id.ToString())
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(structureJson))
            {
                return map;
            }

            MappingStructureNode root;
            try
            {
                root = ArcJson.Deserialize<MappingStructureNode>(structureJson);
            }
            catch (Exception ex)
            {
                _logWriter.LogError($"Custom import: failed to parse mapping structure for unique references: {ex.Message}",
                    nameof(UniqueReferenceResolver), nameof(Resolve));
                return map;
            }

            Walk(root, fieldsById, map);
            _logWriter.LogInfo(
                $"Custom import unique references resolved: patient='{map.PatientField}', specimen='{map.SpecimenField}', culture='{map.CultureField}'",
                nameof(UniqueReferenceResolver), nameof(Resolve));
            return map;
        }

        private void Walk(MappingStructureNode node, IReadOnlyDictionary<string, ExportProfileFieldModel> fieldsById, UniqueReferenceMap map)
        {
            if (node == null)
            {
                return;
            }

            if (string.Equals(node.Kind, "attribute", StringComparison.OrdinalIgnoreCase)
                && string.Equals(node.UniqueReference?.Trim(), "Yes", StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrWhiteSpace(node.FieldKey))
            {
                var fieldId = FieldIdFromKey(node.FieldKey);
                if (fieldsById.TryGetValue(fieldId, out var field) && !string.IsNullOrWhiteSpace(field.FieldName))
                {
                    var bucket = UniqueReferenceTableBucket.Normalize(field.TableName);
                    switch (bucket)
                    {
                        case "patient": map.PatientField = field.FieldName; break;
                        case "specimen": map.SpecimenField = field.FieldName; break;
                        case "culture": map.CultureField = field.FieldName; break;
                    }
                }
            }

            foreach (var child in node.Children ?? new List<MappingStructureNode>())
            {
                Walk(child, fieldsById, map);
            }
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
