using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig;

/// <summary>
/// Query that retrieves a list of workflow options for dropdown lists from the database.
/// </summary>
internal class WorkflowListQueryForDropdownQuery : IQueryReturningType<List<OptionsConfig>>
{
    /// <summary>
    /// Executes the dropdown workflow query asynchronously.
    /// </summary>
    /// <param name="connect">An open <see cref="NpgsqlConnection"/> used to execute the SQL query.</param>
    /// <param name="queryFilters">A configuration object for query filters (currently unused).</param>
    /// <returns>
    /// A task representing the asynchronous operation, whose result is a list of 
    /// <see cref="OptionsConfig"/> objects containing the key and text for dropdown options.
    /// </returns>
    public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var sql = @"select id as key, contents->>'Description' as Text from configs where configTypeid = 18";
        var result = await connect.QueryAsync<OptionsConfig>(sql);
        return result.ToList();
    }
}
