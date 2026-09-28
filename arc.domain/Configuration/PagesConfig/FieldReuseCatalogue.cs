using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.PagesConfig;

/// <summary>
/// Read only catalogue describing which fields may be reused across forms. Reuse is keyed on the
/// entity table a page stores to: any two pages resolving to the same table may share fields,
/// wherever those pages live. The rule is deliberately generic rather than a list of known
/// tables, so opening form configuration up to further views needs no change here.
/// </summary>
public static class FieldReuseCatalogue
{
    /// <summary>
    /// Entity tables that never take part in field reuse. Direct test and culture test forms
    /// resolve to these, and reuse is meaningless there: each test stores its own record, so a
    /// shared field id would not share a value, and the tests are configured per test anyway.
    /// This is the only exception to the generic rule.
    /// </summary>
    public static readonly IReadOnlyCollection<string> NonReusableTables =
        ["Tests", "CultureTests"];

    /// <summary>
    /// Field ids the system manages and that must never be offered for reuse.
    /// </summary>
    public static readonly IReadOnlyCollection<string> ReservedFieldIds =
        ["Id", "PrintOnReport"];

    /// <summary>
    /// Control types that configure the form itself rather than store data.
    /// </summary>
    public static readonly IReadOnlyCollection<string> NonDataFieldTypes =
        ["ruleseditor", "pagerules", "fieldselector", "parentlinkeditor",
         "existingfieldselector", "pageorder", "text"];

    /// <summary>
    /// Sentinel <c>PageConfig.TableName</c> marking a page that stores nothing, used by search
    /// and selection pages whose fields are query criteria rather than stored data.
    /// </summary>
    public const string NoTableSentinel = "None";

    /// <summary>
    /// Returns true when a page resolving to <paramref name="tableName"/> may take part in field
    /// reuse, either as a target or as a source of candidates. A page with no entity table has
    /// nothing to share, and the tables in <see cref="NonReusableTables"/> are excluded outright.
    /// </summary>
    /// <param name="tableName">Resolved entity table name, or null when the page is unscoped.</param>
    /// <returns>True when pages on this table may share fields.</returns>
    public static bool IsReusableTable(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
        {
            return false;
        }

        var trimmed = tableName.Trim();

        if (NoTableSentinel.Equals(trimmed, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return !NonReusableTables.Any(t => t.Equals(trimmed, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Returns true when a page may contribute reuse candidates. Search, selection and results
    /// pages are excluded because their fields are query criteria with no column and no MoreData
    /// key, so placing them on a data page would create a field that never saves. Such pages
    /// declare themselves with the <c>None</c> table name sentinel.
    /// <para>
    /// <c>PageConfig.ConfigureActions</c> is deliberately not consulted. It records whether a
    /// page's own layout may be changed, which is locked down on pages that are fixed for business
    /// or system reasons. Those pages still hold ordinary stored fields, so their fields remain
    /// available to other forms.
    /// </para>
    /// </summary>
    /// <param name="pageTableName">Raw <c>PageConfig.TableName</c> of the source page.</param>
    /// <returns>True when the page's fields may be offered for reuse.</returns>
    public static bool IsCandidateSourcePage(string pageTableName) =>
        !NoTableSentinel.Equals(pageTableName?.Trim(), StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns true when a field may be offered as a reuse candidate. Fields marked
    /// <c>Configurable: 'No'</c> are deliberately included, because canned identity fields such
    /// as <c>PatientRef</c> and <c>Surname</c> are common reuse targets; the flag is copied with
    /// the field so it stays non editable in the designer once placed.
    /// </summary>
    /// <param name="fieldId">Candidate field id.</param>
    /// <param name="fieldType">Candidate field control type.</param>
    /// <returns>True when the field is a reusable data field.</returns>
    public static bool IsReusable(string fieldId, string fieldType)
    {
        if (string.IsNullOrWhiteSpace(fieldId))
        {
            return false;
        }

        if (ReservedFieldIds.Any(r => r.Equals(fieldId.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        return string.IsNullOrWhiteSpace(fieldType)
            || !NonDataFieldTypes.Any(t => t.Equals(fieldType.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}
