using arc.common.ExtensionMethods;
using arc.common.Models.Coding;
using arc.data.Extensions;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specification;

/// <summary>
/// Data query that loads the specification list with optional filters for the specification list view.
/// </summary>
/// <remarks>
/// Supports filtering by guidelinesid (multi-select) and text search across guidelines, document,
/// version number, and publication year display values. Results are ordered per query filters and limited to 500 rows.
/// </remarks>
internal class SpecificationListQuery : IQueryReturningType<List<SpecificationListModel>>
{
    /// <summary>
    /// Executes the specification list query with the given filters and returns matching specifications.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Optional filters: "guidelinesid", "documentid", "versionnumberid", "publicationyearid" (comma-separated IDs), "searchText" (text search across all display values).</param>
    /// <returns>List of specification rows mapped to <see cref="SpecificationListModel"/> (max 500).</returns>
    public async Task<List<SpecificationListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var queryFilterHasGuidelinesId = queryFilters.TryGetStringValue("guidelinesid", out var guidelinesId, "");
        var guidelinesIds = guidelinesId.ToIntList().ToArray();
        var queryFilterHasDocumentId = queryFilters.TryGetStringValue("documentid", out var documentId, "");
        var documentIds = documentId.ToIntList().ToArray();
        var queryFilterHasVersionNumberId = queryFilters.TryGetStringValue("versionnumberid", out var versionNumberId, "");
        var versionNumberIds = versionNumberId.ToIntList().ToArray();
        var queryFilterHasPublicationYearId = queryFilters.TryGetStringValue("publicationyearid", out var publicationYearId, "");
        var publicationYearIds = publicationYearId.ToIntList().ToArray();

        var searchText = queryFilters.GetStringValue("searchtext") ?? "";
        var wildcardSearchText = searchText.ToLower().Trim().ToSqlWildcard();

        var conditions = new List<string>();
        if (queryFilterHasGuidelinesId && guidelinesIds.Length != 0)
        {
            conditions.Add("s.guidelinesid = ANY(@guidelinesIds)");
        }
        if (queryFilterHasDocumentId && documentIds.Length != 0)
        {
            conditions.Add("s.documentid = ANY(@documentIds)");
        }
        if (queryFilterHasVersionNumberId && versionNumberIds.Length != 0)
        {
            conditions.Add("s.versionnumberid = ANY(@versionNumberIds)");
        }
        if (queryFilterHasPublicationYearId && publicationYearIds.Length != 0)
        {
            conditions.Add("s.publicationyearid = ANY(@publicationYearIds)");
        }
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            conditions.Add("(l1.value ilike @wildcardSearchText or l2.value ilike @wildcardSearchText or l3.value ilike @wildcardSearchText or l4.value ilike @wildcardSearchText)");
        }
        var wherePart = conditions.Count > 0 ? " where " + string.Join(" and ", conditions) : "";

        var sql = $"""
            select s.id, guidelinesid, documentid, versionnumberid, publicationyearid, l1.value as guidelines, l2.value as document, l3.value as versionnumber, l4.value as publicationyear
            from specification s
            left outer join listitem l1 on l1.id = guidelinesid
            left outer join listitem l2 on l2.id = documentid
            left outer join listitem l3 on l3.id = versionnumberid
            left outer join listitem l4 on l4.id = publicationyearid
            {wherePart}
            order by {ListQueryOrderByUtil.GetOrderByClause(queryFilters, "s.lastmodifieddate")}
            limit 500
            """;

        object parameters;
        var hasListFilters = (queryFilterHasGuidelinesId && guidelinesIds.Length != 0) || (queryFilterHasDocumentId && documentIds.Length != 0)
            || (queryFilterHasVersionNumberId && versionNumberIds.Length != 0) || (queryFilterHasPublicationYearId && publicationYearIds.Length != 0);
        if (hasListFilters && !string.IsNullOrWhiteSpace(searchText))
        {
            var dict = new Dictionary<string, object> { { "wildcardSearchText", wildcardSearchText } };
            if (queryFilterHasGuidelinesId && guidelinesIds.Length != 0) dict["guidelinesIds"] = guidelinesIds;
            if (queryFilterHasDocumentId && documentIds.Length != 0) dict["documentIds"] = documentIds;
            if (queryFilterHasVersionNumberId && versionNumberIds.Length != 0) dict["versionNumberIds"] = versionNumberIds;
            if (queryFilterHasPublicationYearId && publicationYearIds.Length != 0) dict["publicationYearIds"] = publicationYearIds;
            parameters = dict;
        }
        else if (hasListFilters)
        {
            var dict = new Dictionary<string, object>();
            if (queryFilterHasGuidelinesId && guidelinesIds.Length != 0) dict["guidelinesIds"] = guidelinesIds;
            if (queryFilterHasDocumentId && documentIds.Length != 0) dict["documentIds"] = documentIds;
            if (queryFilterHasVersionNumberId && versionNumberIds.Length != 0) dict["versionNumberIds"] = versionNumberIds;
            if (queryFilterHasPublicationYearId && publicationYearIds.Length != 0) dict["publicationYearIds"] = publicationYearIds;
            parameters = dict;
        }
        else if (!string.IsNullOrWhiteSpace(searchText))
        {
            parameters = new { wildcardSearchText };
        }
        else
        {
            parameters = null;
        }

        var result = parameters != null
            ? await connect.QueryAsync<SpecificationListModel>(sql, parameters)
            : await connect.QueryAsync<SpecificationListModel>(sql);
        return result.ToList();
    }
}
