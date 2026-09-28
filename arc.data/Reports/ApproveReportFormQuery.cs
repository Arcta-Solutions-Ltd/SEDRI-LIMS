using arc.common.Models.Reports;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Reports;

/// <summary>
/// Represents a query that retrieves the approval form data for a given report history entry.
/// </summary>
internal class ApproveReportFormQuery : IQueryReturningType<ApproveReportFormModel>
{
    /// <summary>
    /// Executes the database query to fetch the <see cref="ApproveReportFormModel"/> 
    /// based on the 'id' filter provided in <paramref name="queryFilters"/>.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> to the target database.
    /// </param>
    /// <param name="queryFilters">
    /// Configuration containing the filter criteria, including the 'id' value.
    /// </param>
    /// <returns>
    /// A task that returns an <see cref="ApproveReportFormModel"/> populated 
    /// with the specimen ID and report display ID.
    /// </returns>
    public async Task<ApproveReportFormModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");

        var sql = $"""
            select specimenid as id, id as reportdisplayid from reporthistory where id = @Id
            """;

        var result = await connect.QueryFirstAsync<ApproveReportFormModel>(sql, new { id });
        return result;
    }
}
