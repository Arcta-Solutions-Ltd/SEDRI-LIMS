using arc.common.Models.Export;
using System;
using System.Collections.Generic;

namespace arc.app.Exports
{
    /// <summary>
    /// Normalizes export profile field table names into the buckets used to enforce
    /// at-most-one Unique Reference per source table group in a mapping.
    /// </summary>
    public static class UniqueReferenceTableBucket
    {
        /// <summary>
        /// Maps an export profile <see cref="ExportProfileMappingFieldOption.TableName"/> (or raw table
        /// segment from a field key) to the canonical unique-reference bucket.
        /// </summary>
        public static string Normalize(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return string.Empty;
            }

            return tableName.Trim().ToLowerInvariant() switch
            {
                "patient" => "patient",
                "specimen" or "tests" => "specimen",
                "culture" or "culturetests" => "culture",
                "ast" => "ast",
                "custom" => "custom",
                _ => tableName.Trim().ToLowerInvariant()
            };
        }

        /// <summary>
        /// Resolves the unique-reference bucket for an attribute from its bindings.
        /// Returns null when the attribute is unmapped or the binding cannot be resolved.
        /// </summary>
        public static string ResolveAttributeBucket(
            string fieldKey,
            string gridSubFieldId,
            string parentGridFieldKey,
            IReadOnlyDictionary<string, ExportProfileMappingFieldOption> fieldOptionsByKey)
        {
            var tableName = ResolveTableName(fieldKey, gridSubFieldId, parentGridFieldKey, fieldOptionsByKey);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return null;
            }

            return Normalize(tableName);
        }

        /// <summary>
        /// Returns a user-facing label for validation errors.
        /// </summary>
        public static string DescribeBucket(string bucket)
        {
            if (string.IsNullOrWhiteSpace(bucket))
            {
                return "table";
            }

            return bucket.ToLowerInvariant() switch
            {
                "patient" => "patient",
                "specimen" => "specimen",
                "culture" => "culture",
                "ast" => "AST",
                "custom" => "custom",
                _ => bucket
            };
        }

        private static string ResolveTableName(
            string fieldKey,
            string gridSubFieldId,
            string parentGridFieldKey,
            IReadOnlyDictionary<string, ExportProfileMappingFieldOption> fieldOptionsByKey)
        {
            if (!string.IsNullOrWhiteSpace(fieldKey))
            {
                if (fieldOptionsByKey != null && fieldOptionsByKey.TryGetValue(fieldKey, out var option)
                    && !string.IsNullOrWhiteSpace(option?.TableName))
                {
                    return option.TableName;
                }

                return ParseTableNameFromFieldKey(fieldKey);
            }

            if (!string.IsNullOrWhiteSpace(gridSubFieldId) && !string.IsNullOrWhiteSpace(parentGridFieldKey)
                && fieldOptionsByKey != null && fieldOptionsByKey.TryGetValue(parentGridFieldKey, out var gridOption)
                && !string.IsNullOrWhiteSpace(gridOption?.TableName))
            {
                return gridOption.TableName;
            }

            return null;
        }

        private static string ParseTableNameFromFieldKey(string fieldKey)
        {
            var parts = fieldKey?.Split('|') ?? Array.Empty<string>();
            return parts.Length >= 3 ? parts[2] : null;
        }
    }
}
