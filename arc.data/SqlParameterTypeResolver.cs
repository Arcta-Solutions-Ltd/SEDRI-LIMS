using arc.common.ExtensionMethods;
using arc.data.model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace arc.data
{
    internal sealed record FieldTypeInfo(Type ClrType, bool IsJsonb, bool IsImmutableOnUpdate);

    /// <summary>
    /// Resolves CLR types for table fields using <c>arc.data.model</c> data models and converts raw string inputs into typed parameter values.
    /// </summary>
    internal static class SqlParameterTypeResolver
    {
        private static readonly ConcurrentDictionary<string, Dictionary<string, FieldTypeInfo>> FieldTypeMapByTable =
            new(StringComparer.OrdinalIgnoreCase);

        private static readonly ConcurrentDictionary<string, Type?> ModelTypeByTable =
            new(StringComparer.OrdinalIgnoreCase);

        internal static bool TryGetFieldTypeInfo(string tableName, string fieldName, out FieldTypeInfo fieldTypeInfo)
        {
            fieldTypeInfo = default!;

            if (string.IsNullOrWhiteSpace(tableName) || string.IsNullOrWhiteSpace(fieldName))
            {
                return false;
            }

            var map = FieldTypeMapByTable.GetOrAdd(tableName, BuildFieldTypeMapForTable);
            return map.TryGetValue(fieldName, out fieldTypeInfo);
        }

        internal static bool IsImmutableOnUpdate(string tableName, string fieldName)
        {
            return TryGetFieldTypeInfo(tableName, fieldName, out var fieldTypeInfo)
                && fieldTypeInfo.IsImmutableOnUpdate;
        }

        internal static bool TryConvert(string rawValue, FieldTypeInfo fieldTypeInfo, out object? typedValue)
        {
            typedValue = null;

            if (rawValue == null)
            {
                return true;
            }

            var trimmed = rawValue.Trim();
            if (trimmed.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (fieldTypeInfo.IsJsonb)
            {
                // Keep existing behavior: strip nulls and normalize quotes for JSONB storage
                typedValue = trimmed.RemoveNullsFromJsonString().Replace("'", "\"");
                return true;
            }

            var targetType = Nullable.GetUnderlyingType(fieldTypeInfo.ClrType) ?? fieldTypeInfo.ClrType;

            if (targetType == typeof(string))
            {
                typedValue = trimmed;
                return true;
            }

            if (targetType == typeof(bool))
            {
                if (TryParseBoolean(trimmed, out var boolValue))
                {
                    typedValue = boolValue;
                    return true;
                }

                return false;
            }

            if (targetType == typeof(int))
            {
                if (int.TryParse(trimmed, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                {
                    typedValue = intValue;
                    return true;
                }

                return false;
            }

            if (targetType == typeof(long))
            {
                if (long.TryParse(trimmed, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
                {
                    typedValue = longValue;
                    return true;
                }

                return false;
            }

            if (targetType == typeof(decimal))
            {
                if (decimal.TryParse(trimmed, NumberStyles.Number, CultureInfo.InvariantCulture, out var decimalValue))
                {
                    typedValue = decimalValue;
                    return true;
                }

                return false;
            }

            if (targetType == typeof(double))
            {
                if (double.TryParse(trimmed, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var doubleValue))
                {
                    typedValue = doubleValue;
                    return true;
                }

                return false;
            }

            if (targetType == typeof(DateTime) || targetType == typeof(DateOnly))
            {
                // Heuristic: preserve prior "Dates" behavior by treating YYYY-MM-DD as date-only.
                if (TryParseDateOnly(trimmed, out var dateOnly))
                {
                    typedValue = dateOnly;
                    return true;
                }

                if (DateTime.TryParse(trimmed, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dateTimeValue) ||
                    DateTime.TryParse(trimmed, out dateTimeValue))
                {
                    typedValue = dateTimeValue;
                    return true;
                }

                return false;
            }

            if (targetType == typeof(Guid))
            {
                if (Guid.TryParse(trimmed, out var guidValue))
                {
                    typedValue = guidValue;
                    return true;
                }

                return false;
            }

            // Safe fallback: leave as string so Dapper/Npgsql can still bind.
            typedValue = trimmed;
            return true;
        }

        private static Dictionary<string, FieldTypeInfo> BuildFieldTypeMapForTable(string tableName)
        {
            var modelType = ModelTypeByTable.GetOrAdd(tableName, ResolveModelType);
            if (modelType == null)
            {
                return new Dictionary<string, FieldTypeInfo>(StringComparer.OrdinalIgnoreCase);
            }

            var map = new Dictionary<string, FieldTypeInfo>(StringComparer.OrdinalIgnoreCase);

            foreach (var prop in modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                // Property name matching is case-insensitive against payload keys.
                var isJsonb = prop.GetCustomAttributes(inherit: true).Any(a => a.GetType() == typeof(JsonbAttribute));
                var isImmutableOnUpdate = prop.GetCustomAttributes(inherit: true).Any(a => a.GetType() == typeof(ImmutableOnUpdateAttribute));
                map[prop.Name] = new FieldTypeInfo(prop.PropertyType, isJsonb, isImmutableOnUpdate);
            }

            return map;
        }

        private static Type? ResolveModelType(string tableName)
        {
            var tablesToCopy = DataModelUtils.DataModelList();
            var table = tablesToCopy.FirstOrDefault(d => d.Value.Equals(tableName, StringComparison.OrdinalIgnoreCase));
            return table.Key;
        }

        private static bool TryParseBoolean(string value, out bool parsed)
        {
            parsed = false;

            if (bool.TryParse(value, out parsed))
            {
                return true;
            }

            if (value.Equals("1", StringComparison.OrdinalIgnoreCase) || value.Equals("yes", StringComparison.OrdinalIgnoreCase) || value.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                parsed = true;
                return true;
            }

            if (value.Equals("0", StringComparison.OrdinalIgnoreCase) || value.Equals("no", StringComparison.OrdinalIgnoreCase) || value.Equals("n", StringComparison.OrdinalIgnoreCase))
            {
                parsed = false;
                return true;
            }

            return false;
        }

        private static bool TryParseDateOnly(string value, out DateTime dateOnly)
        {
            dateOnly = default;

            if (value.Length != 10)
            {
                return false;
            }

            return DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOnly);
        }
    }
}
