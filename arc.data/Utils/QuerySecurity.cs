using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Text;

namespace arc.data.Utils;

/// <summary>
/// Provides security-related query building utilities.
/// All user input is sanitized to prevent SQL injection attacks.
/// </summary>
internal class QuerySecurity
{
    /// <summary>
    /// Builds a WHERE clause for laboratory or organisation filtering.
    /// All ID lists are sanitized to only allow valid integers.
    /// </summary>
    /// <param name="queryFilters">The query filter configuration.</param>
    /// <returns>A sanitized WHERE clause string.</returns>
    public static string GetWhereClause(QueryFilterConfig queryFilters)
    {
        var whereClause = new StringBuilder("");
        var queryFiltersHasLaboratoryId = queryFilters.TryGetStringValue("laboratoryid", out var laboratoryIdCsv);
        var queryFiltersHasOrganisationId = queryFilters.TryGetStringValue("organisationid", out var organisationIdCsv);

        //if (!queryFiltersHasLaboratoryId && !queryFiltersHasOrganisationId)
        //{
        //    throw new System.Exception("Query cannot be run without an associated laboratory or organisation id.");
        //}

        if (queryFiltersHasLaboratoryId)
        {
            // Sanitize ID list to only allow valid integers - prevents SQL injection
            var sanitizedLaboratoryIds = SqlSanitizer.SanitizeIdList(laboratoryIdCsv);
            whereClause.Append($"laboratoryid in ({sanitizedLaboratoryIds})");
        }
        else
        {
            if (queryFiltersHasOrganisationId)
            {
                // Sanitize ID list to only allow valid integers - prevents SQL injection
                var sanitizedOrganisationIds = SqlSanitizer.SanitizeIdList(organisationIdCsv);
                whereClause.Append($"organisationid in ({sanitizedOrganisationIds})");
            }
        }

        return whereClause.ToString();
    }
}
