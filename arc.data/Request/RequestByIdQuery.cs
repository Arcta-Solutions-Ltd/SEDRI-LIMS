using arc.common.Models.Requests;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Request;

/// <summary>
/// Returns a single request by its primary key.
/// </summary>
internal class RequestByIdQuery : IQueryReturningType<RequestModel>
{
    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the id parameter.</param>
    /// <returns>The request, or an empty model when no row matches.</returns>
    public async Task<RequestModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");

        var sql = "SELECT Id, PatientId, AdmissionId, RequestId, MoreData, LastModifiedDate FROM Request WHERE Id = @Id";

        var result = await connect.QueryFirstOrDefaultAsync<RequestModel>(sql, new { Id = id });

        return result ?? new RequestModel();
    }
}
