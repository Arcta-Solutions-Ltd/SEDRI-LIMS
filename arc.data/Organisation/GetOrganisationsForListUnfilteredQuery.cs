using arc.common.ExtensionMethods;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Organisation;

/// <summary>
/// Provides a query for retrieving an enabled list of organisations formatted as options.
/// </summary>
/// <remarks>
/// Unfiltered by token or hierarchy scope, but limited to enabled organisations only.
/// This query selects the organisation's ID, fully qualified name, and parent organisation ID,
/// then maps these to the properties of <see cref="OptionsConfig"/>.
/// The results are ordered by the fully qualified name.
/// </remarks>
internal class GetOrganisationsForListUnfilteredQuery : IQueryReturningType<List<OptionsConfig>>
{
    /// <summary>
    /// Executes the query asynchronously to retrieve a list of enabled organisations.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> used to execute the SQL query.
    /// </param>
    /// <param name="queryFilters">
    /// The query filters; not used in this query but required by the interface.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="OptionsConfig"/> objects.
    /// </returns>
    public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var enabledPredicate = OrganisationEnabledExtensions.EnabledOnlyPredicate();
        var sql = $@"select id as key, fullyqualifiedname as text, parentorganisationid as ParentKey 
                    from organisation 
                    where {enabledPredicate}
                    order by fullyqualifiedname";

        var result = await connect.QueryAsync<OptionsConfig>(sql);
        return result.ToList();
    }
}
