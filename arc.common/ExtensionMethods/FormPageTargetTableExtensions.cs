using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Helpers for page-level table targets on specimen add/edit forms.
/// </summary>
public static class FormPageTargetTableExtensions
{
    /// <summary>
    /// Supported page TableName values for multi-entity specimen forms.
    /// </summary>
    public static readonly IReadOnlyCollection<string> SpecimenFormTargets =
        ["specimen", "patient", "admission", "request"];

    /// <summary>
    /// Returns true when the form's <c>SingleItemName</c> identifies a specimen record form.
    /// Page table routing applies only to these forms.
    /// </summary>
    /// <param name="singleItemName">Form <c>SingleItemName</c> value.</param>
    /// <returns>True when the form creates or edits specimen records.</returns>
    public static bool IsSpecimenRecordForm(string singleItemName) =>
        string.Equals(singleItemName?.Trim(), "specimen", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns true when <paramref name="tableName"/> is one of the four specimen-form targets.
    /// </summary>
    /// <param name="tableName">Page or table name to validate.</param>
    /// <returns>True when the value is a supported target.</returns>
    public static bool IsSpecimenFormTarget(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
        {
            return false;
        }

        return SpecimenFormTargets.Any(
            t => t.Equals(tableName.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Normalizes a table name to lowercase trimmed form.
    /// </summary>
    /// <param name="tableName">Raw table name.</param>
    /// <returns>Normalized table name or empty string.</returns>
    public static string Normalize(string tableName) =>
        string.IsNullOrWhiteSpace(tableName) ? string.Empty : tableName.Trim().ToLowerInvariant();

    /// <summary>
    /// Resolves the effective page target; empty page TableName defaults to specimen on specimen forms.
    /// </summary>
    /// <param name="pageTableName">Value from PageConfig.TableName.</param>
    /// <param name="defaultTable">Default when page TableName is unset.</param>
    /// <returns>Resolved target table name.</returns>
    public static string ResolvePageTarget(string pageTableName, string defaultTable = "specimen")
    {
        if (string.IsNullOrWhiteSpace(pageTableName))
        {
            return Normalize(defaultTable);
        }

        var normalized = Normalize(pageTableName);
        return IsSpecimenFormTarget(normalized) ? normalized : Normalize(defaultTable);
    }

    /// <summary>
    /// Returns true when the resolved page target differs from the form primary save table.
    /// </summary>
    /// <param name="pageTableName">Page TableName value.</param>
    /// <param name="primarySaveTable">Event primary table (usually specimen).</param>
    /// <returns>True when fields on the page route to a non-primary entity.</returns>
    public static bool IsNonPrimaryTarget(string pageTableName, string primarySaveTable = "specimen")
    {
        var resolved = ResolvePageTarget(pageTableName, primarySaveTable);
        return !resolved.Equals(Normalize(primarySaveTable), StringComparison.OrdinalIgnoreCase);
    }
}
