using arc.common.ExtensionMethods;
using arc.data.Utils;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Organisation;

/// <summary>
/// Query for retrieving a list of enabled organisations for a dropdown or similar selection.
/// </summary>
internal class GetOrganisationsForListQuery : IQueryReturningType<List<OptionsConfig>>
{
    /// <summary>
    /// Executes the query to retrieve a list of enabled organisations based on filter criteria.
    /// Lab-scoped users receive all enabled organisations; org-scoped users receive enabled
    /// organisations within their hierarchy.
    /// </summary>
    /// <param name="connect">The database connection to use for the query.</param>
    /// <param name="queryFilters">The query filter configuration containing parameters like LaboratoryId and OrganisationId.</param>
    /// <returns>
    /// A list of <see cref="OptionsConfig"/> objects representing organisations,
    /// or an empty list if only one organisation matches the criteria.
    /// </returns>
    public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var labid = queryFilters.Parameters.Where(p => p.Key.Equals("laboratoryid", System.StringComparison.CurrentCultureIgnoreCase)).First();
        var orgid = queryFilters.Parameters.Where(p => p.Key.Equals("organisationid", System.StringComparison.CurrentCultureIgnoreCase)).First();
        var enabledPredicate = OrganisationEnabledExtensions.EnabledOnlyPredicate();
        var whereClause = " where " + enabledPredicate;

        if (string.IsNullOrWhiteSpace(labid.Value))
        {
            var orgIds = string.Join(",", await new OrganisationIdListFromOrganisationIdQuery().ExecuteAsync(connect, int.Parse(orgid.Value)));
            whereClause = " where Id in (" + orgIds + ") and " + enabledPredicate;
        }

        var sql = @"select id as key, fullyqualifiedname || case when code is null then '' else ' (' || code || ')' end as text, parentorganisationid as ParentKey from organisation"
                    + whereClause +
                  " order by fullyqualifiedname";
        var result = await connect.QueryAsync<OptionsConfig>(sql);
        return result.Count() == 1 ? [] : result.ToList();
    }
}
