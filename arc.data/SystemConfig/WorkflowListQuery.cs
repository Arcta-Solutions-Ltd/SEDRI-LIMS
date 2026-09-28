using arc.common.Models.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig;

/// <summary>
/// A query for retrieving a list of workflows from the PostgreSQL database.
/// </summary>
internal class WorkflowListQuery : IQueryReturningType<List<WorkflowListModel>>
{
    /// <summary>
    /// Executes the workflow list query asynchronously.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> used to execute the SQL query.
    /// </param>
    /// <param name="queryFilters">
    /// A <see cref="QueryFilterConfig"/> object for filtering the query results (currently unused).
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="WorkflowListModel"/>
    /// populated by executing the SQL query.
    /// </returns>
    public async Task<List<WorkflowListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var sql = @"select id, contents->>'Name' as Name, contents->>'Description' As Description
                    from configs where configTypeid = 18";

        var result = await connect.QueryAsync<WorkflowListModel>(sql);
        return result.ToList();
    }
}

