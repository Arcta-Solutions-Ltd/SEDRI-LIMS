using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using arc.app.Common;
using arc.common.Models.Instruments.CustomImport;
using arc.domain.Instruments;

namespace arc.app.Instruments.CustomImport
{
    /// <summary>
    /// Default <see cref="IInterfaceCriteriaEvaluator"/>. Flattens the record's field values (keyed by export profile
    /// field id) and evaluates each criteria row, resolving list values to ids before comparison. Rows are combined with
    /// And (987) or Or (988).
    /// </summary>
    public class InterfaceCriteriaEvaluator : IInterfaceCriteriaEvaluator
    {
        private const string AndRuleId = "987";
        private const string OrRuleId = "988";

        private readonly IImportValueResolver _valueResolver;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceCriteriaEvaluator"/> class.
        /// </summary>
        /// <param name="valueResolver">Resolver used to convert imported list values to ids before comparison.</param>
        /// <param name="logWriter">Log writer for installed-system diagnostics.</param>
        public InterfaceCriteriaEvaluator(IImportValueResolver valueResolver, ILogWriter logWriter)
        {
            _valueResolver = valueResolver;
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public async Task<bool> IsMatchAsync(SingleInstrumentConfig profile, ImportRecord record)
        {
            var criteria = profile?.InterfaceCriteria?.Where(c => c != null && !string.IsNullOrWhiteSpace(c.Field)).ToList()
                           ?? new List<InterfaceCriteriaLine>();
            if (criteria.Count == 0)
            {
                _logWriter.LogInfo("Custom import: no interface criteria; record matches", nameof(InterfaceCriteriaEvaluator), nameof(IsMatchAsync));
                return true;
            }

            var valuesByFieldId = FlattenFields(record);
            var isAnd = !string.Equals(profile.InterfaceCriteriaAndOr?.Trim(), OrRuleId, StringComparison.Ordinal);

            bool aggregate = isAnd;
            foreach (var line in criteria)
            {
                var matched = await EvaluateLineAsync(line, valuesByFieldId);
                if (isAnd)
                {
                    aggregate &= matched;
                    if (!aggregate) break;
                }
                else
                {
                    aggregate |= matched;
                    if (aggregate) break;
                }
            }

            _logWriter.LogInfo(
                $"Custom import criteria evaluated: rule={(isAnd ? "And" : "Or")}, rows={criteria.Count}, match={aggregate}",
                nameof(InterfaceCriteriaEvaluator), nameof(IsMatchAsync));
            return aggregate;
        }

        private async Task<bool> EvaluateLineAsync(InterfaceCriteriaLine line, IReadOnlyDictionary<string, ImportFieldValue> valuesByFieldId)
        {
            if (!valuesByFieldId.TryGetValue(line.Field.Trim(), out var imported) || imported.Value == null)
            {
                return false;
            }

            var comparison = string.IsNullOrWhiteSpace(line.Comparison) ? "=" : line.Comparison.Trim();

            // List criterion: compare list item ids (resolve the imported value to an id first).
            if (!string.IsNullOrWhiteSpace(line.ListValue))
            {
                var resolved = await _valueResolver.ResolveAsync(imported.Bucket, imported.FieldName, imported.Value);
                return string.Equals(resolved?.Trim(), line.ListValue.Trim(), StringComparison.OrdinalIgnoreCase);
            }

            // Numeric criterion.
            if (!string.IsNullOrWhiteSpace(line.NumberValue))
            {
                if (decimal.TryParse(imported.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var actual)
                    && decimal.TryParse(line.NumberValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var expected))
                {
                    return CompareNumeric(actual, expected, comparison);
                }
                return false;
            }

            // Free-text criterion (equality).
            return string.Equals(imported.Value.Trim(), (line.StringValue ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase);
        }

        private static bool CompareNumeric(decimal actual, decimal expected, string comparison) => comparison switch
        {
            ">" => actual > expected,
            ">=" => actual >= expected,
            "<" => actual < expected,
            "<=" => actual <= expected,
            _ => actual == expected
        };

        private static Dictionary<string, ImportFieldValue> FlattenFields(ImportRecord record)
        {
            var map = new Dictionary<string, ImportFieldValue>(StringComparer.OrdinalIgnoreCase);
            void Collect(ImportEntity entity)
            {
                if (entity == null) return;
                foreach (var field in entity.Fields)
                {
                    var key = field.FieldId.ToString();
                    if (!map.ContainsKey(key))
                    {
                        map[key] = field;
                    }
                }
                foreach (var child in entity.Children)
                {
                    Collect(child);
                }
            }

            Collect(record?.Patient);
            foreach (var specimen in record?.Specimens ?? new List<ImportEntity>())
            {
                Collect(specimen);
            }
            return map;
        }
    }
}
