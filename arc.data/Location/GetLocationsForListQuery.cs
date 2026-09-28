using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Location;

/// <summary>
/// Represents a query for retrieving a list of location options.
/// </summary>
internal class GetLocationsForListQuery : IQueryReturningType<List<OptionsConfig>>
{
    /// <summary>
    /// Executes the query asynchronously to retrieve location options for a list.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="queryFilters">The query filters to apply (not used in this implementation).</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of location options.
    /// </returns>
    public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var sql = @"select id as key, 
                    fullyqualifiedname || case when code is null then '' else ' (' || code || ')' end as text, 
                    parentlocationid as ParentKey
                    from location order by fullyqualifiedname";
        var result = await connect.QueryAsync<OptionsConfig>(sql);
        return result.ToList();
    }
}

