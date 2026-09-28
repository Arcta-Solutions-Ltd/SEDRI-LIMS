using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Data query that loads breakpoint lines (resultline rows) for a given breakpoint.
/// Resolves susceptibility display text from listitem via resultline.resultid.
/// Used by the breakpoint lines list view section on the breakpoint record view.
/// </summary>
internal class BreakpointLineListByBreakpointIdQuery : IQueryReturningType<List<BreakpointLineListModel>>
{
    /// <summary>
    /// Executes the query and returns the list of breakpoint lines for the given breakpoint.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Filter config; expects "BreakpointId" with the breakpoint ID.</param>
    /// <returns>List of breakpoint line rows with susceptibility display text, start value, and end value.</returns>
    public async Task<List<BreakpointLineListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var breakpointId = queryFilters.GetIntegerValue("BreakpointId");

        var sql = """
            select
                rl.Id,
                li.value as susceptibilityname,
                rl.StartVal,
                rl.EndVal
            from resultline rl
            left join listitem li on li.id = rl.resultid
            where rl.breakpointid = @breakpointId
            order by rl.Id
            """;

        var results = await connect.QueryAsync<BreakpointLineListModel>(sql, new { breakpointId });
        return results.ToList();
    }
}
