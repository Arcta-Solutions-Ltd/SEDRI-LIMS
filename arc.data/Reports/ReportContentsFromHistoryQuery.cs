using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Reports;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Reports;

/// <summary>
/// Retrieves the contents of a historical report entry from the ReportHistory table.
/// </summary>
/// <remarks>
/// Implements <see cref="IQueryReturningType{ReportHistory}"/> to enforce returning a <see cref="ReportHistory"/> instance.
/// </remarks>
public class ReportContentsFromHistoryQuery : IQueryReturningType<ReportHistory>
{
    /// <summary>
    /// Executes an asynchronous query using the provided connection and filters to fetch a single report history record.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> against which the query will run.
    /// </param>
    /// <param name="queryFilters">
    /// A <see cref="QueryFilterConfig"/> containing parameters; must include an "id" key.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the matching <see cref="ReportHistory"/> entity.
    /// </returns>
    public async Task<ReportHistory> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

        var sql = @"select name, reportconfig, specimenid, reportapprovalid, contents from ReportHistory where Id = @Id";

        return await connect.QueryFirstAsync<ReportHistory>(sql, new { Id = int.Parse(id.Value) });
    }
}
