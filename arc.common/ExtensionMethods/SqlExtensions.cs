using arc.data.model;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Contains set of static methods to help with sql manipulation.
/// </summary>
public static class SqlExtensions
{
    /// <summary>
    /// Generates a sql statement that can be used by dapper to insert a record using the current object.
    /// </summary>
    /// <param name="obj">Object containing the model generate the sql statement from</param>
    /// <param name="tableName">Table the sql statement will try to insert data into</param>
    /// <returns>String containing the sql statement that has been generated</returns>
    public static string GenerateInsertStatement(this object obj, string tableName)
    {
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();

        // Filter out the "Id" field if its value is 0
        var filteredProperties = properties.Where(p =>
        {
            if (string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase))
            {
                var value = p.GetValue(obj);
                return value is not int intValue || intValue != 0;
            }
            return true;
        }).ToList();

        // Create the insert statement
        StringBuilder sb = new();
        sb.Append($"INSERT INTO {tableName} (");

        // Add column names
        sb.Append(string.Join(", ", filteredProperties.Select(p => p.Name)));
        sb.Append(") VALUES (");

        // Add parameter placeholders
        sb.Append(string.Join(", ", filteredProperties.Select(p =>
        {
            var isJsonb = p.GetCustomAttribute<JsonbAttribute>() != null;
            return isJsonb ? $"CAST(@{p.Name} AS jsonb)" : $"@{p.Name}";
        })));
        sb.Append(")");

        sb.Append(" returning id;");

        var sqlCommand = sb.ToString().Replace("@LastModifiedDate", "now()");


        return sqlCommand;
    }

    /// <summary>
    /// Generates a sql statement that can be used by dapper to update a record using the current object.
    /// </summary>
    /// <param name="obj">Object containing the model generate the sql statement from</param>
    /// <param name="tableName">Table the sql statement will try to insert data into</param>
    /// <param name="keyColumn">The value to use in the where clause</param>
    /// <returns>String containing the sql statement that has been generated</returns>
    public static string GenerateUpdateStatement(this object obj, string tableName, string keyColumn)
    {
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();

        // Build the SQL UPDATE statement
        StringBuilder sql = new();
        sql.Append($"UPDATE {tableName} SET ");

        // Add column-value pairs
        sql.Append(string.Join(", ", properties.Where(p => p.Name != keyColumn).Select(p =>
        {
            var isJsonb = p.GetCustomAttribute<JsonbAttribute>() != null;
            return isJsonb ? $"{p.Name} = CAST(@{p.Name} AS jsonb)" : $"{p.Name} = @{p.Name}";
        })));

        // Add the WHERE clause
        sql.Append($" WHERE {keyColumn} = @{keyColumn};");

        return sql.ToString();
    }
}

/// <summary>
/// Contains static methods for sanitizing SQL input to prevent injection attacks.
/// </summary>
public static class SqlSanitizer
{
    /// <summary>
    /// Sanitizes a comma-separated list of integer IDs for safe use in SQL IN clauses.
    /// </summary>
    /// <param name="commaSeparatedIds">Comma-separated string of IDs to sanitize.</param>
    /// <param name="defaultValue">Value to return if input is empty or invalid (default: "NULL").</param>
    /// <returns>Sanitized comma-separated list of valid integer IDs, or defaultValue if none valid.</returns>
    public static string SanitizeIdList(string commaSeparatedIds, string defaultValue = "NULL")
    {
        if (string.IsNullOrWhiteSpace(commaSeparatedIds))
            return defaultValue;

        var ids = commaSeparatedIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(id => id.Trim())
            .Where(id => int.TryParse(id, out var validId) && validId > 0)
            .ToArray();

        return ids.Length > 0 ? string.Join(",", ids) : defaultValue;
    }

    /// <summary>
    /// Parses a comma-separated list of integer IDs into a sequence of valid integers.
    /// Non-numeric or non-positive values are ignored.
    /// </summary>
    /// <param name="commaSeparatedIds">Comma-separated string of IDs to parse.</param>
    /// <returns>IEnumerable of valid integer IDs.</returns>
    public static IEnumerable<int> ToIntList(this string commaSeparatedIds)
    {
        if (string.IsNullOrWhiteSpace(commaSeparatedIds))
            return Enumerable.Empty<int>();

        return commaSeparatedIds
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(id => id.Trim())
            .Select(id => int.TryParse(id, out var validId) && validId > 0 ? validId : (int?)null)
            .Where(id => id.HasValue)
            .Select(id => id!.Value);
    }

    /// <summary>
    /// Sanitizes a comma-separated list of string values for safe use in SQL IN clauses.
    /// </summary>
    /// <param name="commaSeparatedStrings">Comma-separated string of values to sanitize.</param>
    /// <param name="maxLength">Maximum length for each string value (default: 50).</param>
    /// <param name="defaultValue">Value to return if input is empty or invalid (default: "NULL").</param>
    /// <returns>Sanitized comma-separated list wrapped in single quotes, or defaultValue if none valid.</returns>
    public static string SanitizeStringList(string commaSeparatedStrings, int maxLength = 50, string defaultValue = "NULL")
    {
        if (string.IsNullOrWhiteSpace(commaSeparatedStrings))
            return defaultValue;

        var strings = commaSeparatedStrings.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => s.Length > 0 && s.Length <= maxLength)
            .Where(s => !s.Contains("'") && !s.Contains(";") && !s.Contains("--"))
            .ToArray();

        return strings.Length > 0 ? "'" + string.Join("','", strings) + "'" : defaultValue;
    }

    /// <summary>
    /// Sanitizes a single string value for safe use in SQL LIKE clauses.
    /// </summary>
    /// <param name="searchText">Text to sanitize for LIKE operations.</param>
    /// <param name="maxLength">Maximum length for the search text (default: 100).</param>
    /// <returns>Sanitized search text safe for LIKE operations, or empty string if invalid.</returns>
    public static string SanitizeSearchText(string searchText, int maxLength = 100)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return string.Empty;

        var sanitized = searchText.Trim();
        if (sanitized.Length > maxLength)
            sanitized = sanitized.Substring(0, maxLength);

        // Remove potentially dangerous characters for LIKE operations
        sanitized = sanitized.Replace("'", "''") // Escape single quotes
                            .Replace("%", "\\%") // Escape wildcards
                            .Replace("_", "\\_"); // Escape single character wildcards

        return sanitized;
    }

    /// <summary>
    /// Sanitizes a single string value for safe use in SQL equality comparisons within single quotes.
    /// Escapes single quotes and removes dangerous SQL constructs like comments and semicolons.
    /// </summary>
    /// <param name="value">The string value to sanitize.</param>
    /// <param name="maxLength">Maximum length for the value (default: 255).</param>
    /// <returns>Sanitized string safe for use in SQL queries, or empty string if invalid.</returns>
    public static string SanitizeValue(string value, int maxLength = 255)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var sanitized = value.Trim();
        if (sanitized.Length > maxLength)
            sanitized = sanitized.Substring(0, maxLength);

        // Remove SQL comment sequences
        sanitized = sanitized.Replace("--", "")
                            .Replace("/*", "")
                            .Replace("*/", "");

        // Escape single quotes by doubling them (SQL standard)
        sanitized = sanitized.Replace("'", "''");

        // Remove semicolons to prevent statement termination
        sanitized = sanitized.Replace(";", "");

        return sanitized;
    }

    /// <summary>
    /// Validates and returns a numeric value for safe use in SQL queries without quotes.
    /// Only allows valid integer values to prevent SQL injection.
    /// </summary>
    /// <param name="value">The value to validate as numeric.</param>
    /// <param name="defaultValue">Value to return if input is invalid (default: "0").</param>
    /// <returns>The numeric value as a string if valid, or defaultValue if invalid.</returns>
    public static string SanitizeNumericValue(string value, string defaultValue = "0")
    {
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;

        var trimmed = value.Trim();

        // Allow negative numbers
        if (trimmed.StartsWith("-"))
        {
            if (int.TryParse(trimmed, out var negativeResult))
                return negativeResult.ToString();
        }
        else if (int.TryParse(trimmed, out var result))
        {
            return result.ToString();
        }

        return defaultValue;
    }

    /// <summary>
    /// Validates and returns a numeric value for interval expressions (e.g., days).
    /// Only allows positive integer values.
    /// </summary>
    /// <param name="value">The value to validate as a positive integer.</param>
    /// <param name="defaultValue">Value to return if input is invalid (default: "0").</param>
    /// <returns>The positive integer value as a string if valid, or defaultValue if invalid.</returns>
    public static string SanitizeIntervalValue(string value, string defaultValue = "0")
    {
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;

        var trimmed = value.Trim();

        // Handle ">" prefix for "greater than" comparisons
        if (trimmed.StartsWith(">") && trimmed.Length > 1)
        {
            var numPart = trimmed.Substring(1);
            if (int.TryParse(numPart, out var gtResult) && gtResult >= 0)
                return ">" + gtResult.ToString();
        }
        else if (int.TryParse(trimmed, out var result) && result >= 0)
        {
            return result.ToString();
        }

        return defaultValue;
    }
}
