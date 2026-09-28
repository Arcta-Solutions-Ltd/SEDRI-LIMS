using System;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Resolves the entity scope of a page for field reuse. Scope resolution order is page
/// <c>TableName</c>, then form <c>SingleItemName</c>, then the save event table name.
/// </summary>
public static class FieldReuseScopeExtensions
{
    /// <summary>
    /// Separator between the parts of a field reference key.
    /// </summary>
    public const char ReferenceKeySeparator = '|';

    /// <summary>
    /// Resolves the entity table a page stores its fields in, for field reuse scoping. The most
    /// specific source wins: an explicit page <c>TableName</c>, then the form's
    /// <c>SingleItemName</c>, then the table of the form's save event. Any table name is accepted,
    /// not just a known set, so views added later scope themselves without a code change.
    /// </summary>
    /// <param name="pageTableName">Value of <c>PageConfig.TableName</c>.</param>
    /// <param name="formSingleItemName">Value of <c>FormConfig.SingleItemName</c>.</param>
    /// <param name="saveEventTableName">Value of the form save event <c>TableName</c>.</param>
    /// <returns>The normalized entity table, or null when none of the three is set.</returns>
    public static string ResolveScope(
        string pageTableName,
        string formSingleItemName,
        string saveEventTableName)
    {
        foreach (var candidate in new[] { pageTableName, formSingleItemName, saveEventTableName })
        {
            if (!string.IsNullOrWhiteSpace(candidate))
            {
                return FormPageTargetTableExtensions.Normalize(candidate);
            }
        }

        return null;
    }

    /// <summary>
    /// Returns true when two scopes may share fields. Null scopes never match, so unscoped
    /// pages are handled by the form local rule instead.
    /// </summary>
    /// <param name="targetScope">Target page scope.</param>
    /// <param name="candidateScope">Candidate page scope.</param>
    /// <returns>True when both scopes are the same non null entity table.</returns>
    public static bool ScopesMatch(string targetScope, string candidateScope)
    {
        if (string.IsNullOrWhiteSpace(targetScope) || string.IsNullOrWhiteSpace(candidateScope))
        {
            return false;
        }

        return targetScope.Trim().Equals(candidateScope.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Builds the id based reference key for a field, in the form
    /// <c>{formName}|{pageName}|{fieldId}</c>. All three parts are stable identifiers so the key
    /// is unaffected by label or list item translation.
    /// </summary>
    /// <param name="formName">Source form name id.</param>
    /// <param name="pageName">Source page name id.</param>
    /// <param name="fieldId">Source field id.</param>
    /// <returns>The composite reference key.</returns>
    public static string BuildFieldReferenceKey(string formName, string pageName, string fieldId) =>
        $"{formName}{ReferenceKeySeparator}{pageName}{ReferenceKeySeparator}{fieldId}";

    /// <summary>
    /// Parses a field reference key produced by <see cref="BuildFieldReferenceKey"/>.
    /// </summary>
    /// <param name="referenceKey">The composite reference key.</param>
    /// <returns>The three id parts, or nulls when the key is malformed.</returns>
    public static (string FormName, string PageName, string FieldId) ParseFieldReferenceKey(string referenceKey)
    {
        if (string.IsNullOrWhiteSpace(referenceKey))
        {
            return (null, null, null);
        }

        var parts = referenceKey.Split(ReferenceKeySeparator);
        if (parts.Length < 3)
        {
            return (null, null, null);
        }

        var formName = parts[0].Trim();
        var pageName = parts[1].Trim();
        var fieldId = parts[2].Trim();

        if (formName.Length == 0 || pageName.Length == 0 || fieldId.Length == 0)
        {
            return (null, null, null);
        }

        return (formName, pageName, fieldId);
    }
}
