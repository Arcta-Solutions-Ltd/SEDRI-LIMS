using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Organisation;

/// <summary>
/// Retrieves a comma-separated string of organisation IDs for a given organisation
/// and all its descendants in the hierarchy.
/// </summary>
internal class OrganisationHierarchyListQuery : IQueryReturningString
{
    /// <summary>
    /// Executes the query to build the organisation hierarchy list.
    /// Attempts to parse an "organisationid" filter; if present, it
    /// uses <see cref="OrganisationIdListFromOrganisationIdQuery"/> to
    /// recursively collect all child organisation IDs.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> against which the query will run.
    /// </param>
    /// <param name="queryFilters">
    /// A <see cref="QueryFilterConfig"/> containing parameters.
    /// Must include an "organisationid" integer parameter.
    /// </param>
    /// <returns>
    /// A <see cref="Task{String}"/> that resolves to a comma-separated list of IDs,
    /// or an empty string if the "organisationid" filter is missing.
    /// </returns>
    public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var queryFiltersHasOrganisationId = queryFilters.TryParseIntegerValue("organisationid", out var organisationId);

        if (queryFiltersHasOrganisationId)
        {
            var orgIds = await new OrganisationIdListFromOrganisationIdQuery()
                .ExecuteAsync(connect, organisationId);
            return string.Join(",", orgIds);
        }
        else
        {
            return "";
        }
    }
}
