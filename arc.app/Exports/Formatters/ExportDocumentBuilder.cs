using System;
using System.Collections.Generic;
using System.Linq;
using arc.app.Common;
using arc.common.Models.Export;
using arc.common.Utils;
using Newtonsoft.Json.Linq;

namespace arc.app.Exports.Formatters
{
    /// <summary>
    /// Builds a format-agnostic <see cref="ExportNode"/> document from the post-processed export rows
    /// and a profile's structural mapping tree. Rows are grouped into nested arrays using the hidden
    /// grouping-id columns (specimen / culture / ast id) so nesting is driven by stable ids rather
    /// than by display values (which may be translated). Both the JSON and XML writers serialise the
    /// tree produced here.
    /// </summary>
    public class ExportDocumentBuilder : IExportDocumentBuilder
    {
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportDocumentBuilder"/> class.
        /// </summary>
        /// <param name="logWriter">Log writer used for diagnostics on installed systems.</param>
        public ExportDocumentBuilder(ILogWriter logWriter)
        {
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public ExportNode Build(ExportFormatContext context)
        {
            var columnKeys = context.ColumnKeys ?? new List<string>();
            var dataRows = context.Lines != null && context.Lines.Count > 1
                ? context.Lines.Skip(1).Select(l => l.Split('|')).ToList()
                : new List<string[]>();

            var root = ParseStructure(context.Mapping?.Structure);
            if (root == null)
            {
                _logWriter.LogInfo(
                    $"WARN: Export mapping structure could not be parsed for profile {context.ProfileId}; returning empty document",
                    nameof(ExportDocumentBuilder), nameof(Build));
                return new ExportNode { Name = "root", Type = ExportNodeType.Object };
            }

            _logWriter.LogInfo(
                $"Building mapped export document for profile {context.ProfileId}: rows={dataRows.Count}, columns={columnKeys.Count}",
                nameof(ExportDocumentBuilder), nameof(Build));

            return BuildObject(root, dataRows, columnKeys, isDocumentRoot: true);
        }

        /// <summary>
        /// Deserialises the stored structure JSON into a <see cref="MappingStructureNode"/> tree.
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
                _logWriter.LogError($"Failed to deserialise export mapping structure: {ex.Message}", nameof(ExportDocumentBuilder), nameof(ParseStructure));
                return null;
            }
        }

        /// <summary>
        /// Builds an object node from the supplied rows. At the document root, attribute-only mappings
        /// with multiple rows emit one object per row (CSV row parity). Nested arrays group rows further;
        /// attributes inside a group resolve from the first row unless a sub-array handles expansion.
        /// </summary>
        private ExportNode BuildObject(MappingStructureNode node, List<string[]> rows, List<string> columnKeys, bool isDocumentRoot = false)
        {
            var children = node.Children ?? new List<MappingStructureNode>();
            if (isDocumentRoot && rows.Count > 1 && HasOnlyAttributeChildren(children))
            {
                _logWriter.LogInfo(
                    $"Flat root mapping: emitting {rows.Count} record object(s) as JSON array (CSV row parity)",
                    nameof(ExportDocumentBuilder), nameof(BuildObject));
                return BuildFlatRecordArray(node, rows, columnKeys);
            }

            var result = new ExportNode { Name = string.IsNullOrWhiteSpace(node.Name) ? "root" : node.Name, Type = ExportNodeType.Object };
            foreach (var child in children)
            {
                var built = BuildChild(child, rows, columnKeys);
                if (built != null)
                {
                    result.Children.Add(built);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns true when every child node is an attribute leaf (no nested objects or arrays).
        /// </summary>
        private static bool HasOnlyAttributeChildren(IList<MappingStructureNode> children) =>
            children.Count > 0 && children.All(c =>
                string.Equals(c.Kind, "attribute", StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Builds a top-level array with one object per export row for flat attribute-only mappings.
        /// </summary>
        private ExportNode BuildFlatRecordArray(MappingStructureNode node, List<string[]> rows, List<string> columnKeys)
        {
            var result = new ExportNode
            {
                Name = string.IsNullOrWhiteSpace(node.Name) ? "root" : node.Name,
                Type = ExportNodeType.Array
            };
            foreach (var row in rows)
            {
                var item = new ExportNode { Name = "item", Type = ExportNodeType.Object };
                foreach (var child in node.Children ?? new List<MappingStructureNode>())
                {
                    item.Children.Add(BuildAttribute(child, row, columnKeys));
                }
                result.Children.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Dispatches a single mapping child to the appropriate builder based on its kind.
        /// </summary>
        private ExportNode BuildChild(MappingStructureNode child, List<string[]> rows, List<string> columnKeys)
        {
            switch ((child.Kind ?? string.Empty).ToLowerInvariant())
            {
                case "attribute":
                    return BuildAttribute(child, rows.FirstOrDefault(), columnKeys);
                case "object":
                    return BuildObject(child, rows, columnKeys, isDocumentRoot: false);
                case "array":
                    return BuildArray(child, rows, columnKeys);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Builds a leaf value node from a single row.
        /// </summary>
        private ExportNode BuildAttribute(MappingStructureNode node, string[] row, List<string> columnKeys)
        {
            var value = ResolveValue(node.FieldKey, row, columnKeys);
            return new ExportNode { Name = SafeName(node.Name), Type = ExportNodeType.Value, Value = value };
        }

        /// <summary>
        /// Builds an array node. Grid arrays are sourced from a single grid cell; specimen/culture/ast
        /// arrays group the supplied rows by their hidden id column.
        /// </summary>
        private ExportNode BuildArray(MappingStructureNode node, List<string[]> rows, List<string> columnKeys)
        {
            var arrayType = (node.ArrayType ?? string.Empty).ToLowerInvariant();
            var result = new ExportNode { Name = SafeName(node.Name), Type = ExportNodeType.Array };

            if (arrayType == "grid")
            {
                BuildGridItems(node, rows.FirstOrDefault(), columnKeys, result);
                return result;
            }

            var groupKey = arrayType switch
            {
                "specimens" => ExportColumnKeys.SpecimenId,
                "cultures" => ExportColumnKeys.CultureId,
                "ast" => ExportColumnKeys.AstId,
                _ => null
            };

            if (groupKey == null)
            {
                return result;
            }

            var columnIndex = FindColumnIndex(groupKey, columnKeys);
            if (columnIndex < 0)
            {
                _logWriter.LogInfo(
                    $"WARN: grouping column '{groupKey}' not found for array '{node.Name}'; emitting empty array",
                    nameof(ExportDocumentBuilder), nameof(BuildArray));
                return result;
            }

            var groups = GroupRows(rows, columnIndex);
            foreach (var group in groups)
            {
                var item = BuildObject(new MappingStructureNode { Name = "item", Children = node.Children }, group, columnKeys, isDocumentRoot: false);
                item.Name = "item";
                result.Children.Add(item);
            }

            _logWriter.LogInfo(
                $"Array '{node.Name}' ({arrayType}) produced {result.Children.Count} item(s)",
                nameof(ExportDocumentBuilder), nameof(BuildArray));

            return result;
        }

        /// <summary>
        /// Builds the item objects for a grid array by parsing the grid cell of the supplied row.
        /// </summary>
        private void BuildGridItems(MappingStructureNode node, string[] row, List<string> columnKeys, ExportNode target)
        {
            if (row == null)
            {
                return;
            }
            var gridFieldId = FieldIdFromKey(node.GridFieldKey);
            var columnIndex = FindColumnIndex(gridFieldId, columnKeys);
            if (columnIndex < 0 || columnIndex >= row.Length)
            {
                return;
            }
            var gridRows = ParseGridCell(row[columnIndex]);
            foreach (var gridRow in gridRows)
            {
                var item = new ExportNode { Name = "item", Type = ExportNodeType.Object };
                foreach (var attr in (node.Children ?? new List<MappingStructureNode>()).Where(c => string.Equals(c.Kind, "attribute", StringComparison.OrdinalIgnoreCase)))
                {
                    gridRow.TryGetValue(attr.GridSubFieldId ?? string.Empty, out var value);
                    item.Children.Add(new ExportNode { Name = SafeName(attr.Name), Type = ExportNodeType.Value, Value = value ?? string.Empty });
                }
                target.Children.Add(item);
            }
        }

        /// <summary>
        /// Parses the pseudo-JSON grid cell emitted by GridProcessor (single-quoted
        /// <c>[{'Label' : 'value'}]</c>) into a list of sub-field-id keyed dictionaries.
        /// </summary>
        private List<Dictionary<string, string>> ParseGridCell(string cell)
        {
            var result = new List<Dictionary<string, string>>();
            if (string.IsNullOrWhiteSpace(cell) || cell == "[]")
            {
                return result;
            }
            try
            {
                var asJson = cell.Replace("'", "\"");
                var array = JArray.Parse(asJson);
                foreach (var item in array.OfType<JObject>())
                {
                    var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var prop in item.Properties())
                    {
                        dict[prop.Name] = prop.Value?.ToString() ?? string.Empty;
                    }
                    result.Add(dict);
                }
            }
            catch (Exception ex)
            {
                _logWriter.LogError($"Failed to parse grid cell for mapped export: {ex.Message}", nameof(ExportDocumentBuilder), nameof(ParseGridCell));
            }
            return result;
        }

        /// <summary>
        /// Resolves an attribute value from a row using the field id of the mapping key. Matching is by
        /// field id (never by translated header) so tag/list translations do not break resolution.
        /// </summary>
        private static string ResolveValue(string fieldKey, string[] row, List<string> columnKeys)
        {
            if (row == null)
            {
                return string.Empty;
            }
            var fieldId = FieldIdFromKey(fieldKey);
            var columnIndex = FindColumnIndex(fieldId, columnKeys);
            if (columnIndex < 0 || columnIndex >= row.Length)
            {
                return string.Empty;
            }
            return row[columnIndex];
        }

        /// <summary>
        /// Groups rows by the value of the supplied column index, preserving first-seen order and
        /// skipping rows whose grouping id is empty.
        /// </summary>
        private static List<List<string[]>> GroupRows(List<string[]> rows, int columnIndex)
        {
            var order = new List<string>();
            var map = new Dictionary<string, List<string[]>>();
            foreach (var row in rows)
            {
                if (columnIndex >= row.Length)
                {
                    continue;
                }
                var key = row[columnIndex];
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }
                if (!map.TryGetValue(key, out var list))
                {
                    list = new List<string[]>();
                    map[key] = list;
                    order.Add(key);
                }
                list.Add(row);
            }
            return order.Select(k => map[k]).ToList();
        }

        /// <summary>
        /// Finds the first column whose aligned key equals the supplied field id (case-insensitive).
        /// </summary>
        private static int FindColumnIndex(string fieldId, List<string> columnKeys)
        {
            if (string.IsNullOrWhiteSpace(fieldId))
            {
                return -1;
            }
            for (var i = 0; i < columnKeys.Count; i++)
            {
                if (string.Equals(columnKeys[i], fieldId, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Extracts the field id (first segment) from a canonical mapping key
        /// (<c>{fieldId}|{formName}|{tableName}|{header}</c>).
        /// </summary>
        private static string FieldIdFromKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var pipe = key.IndexOf('|');
            return (pipe < 0 ? key : key.Substring(0, pipe)).Trim();
        }

        /// <summary>
        /// Returns a non-empty node name, defaulting to "value" when the mapping supplied none.
        /// </summary>
        private static string SafeName(string name) => string.IsNullOrWhiteSpace(name) ? "value" : name;
    }
}
