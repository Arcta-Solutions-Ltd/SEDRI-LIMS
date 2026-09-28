using arc.common.Models.Requests;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Request;

/// <summary>
/// Returns the requests held for a single admission, most recent first, for the request selection screen.
/// Used by forms whose request selection is scoped to the admission rather than to the whole patient.
/// </summary>
internal class RequestsForAdmissionQuery : IQueryReturningType<List<RequestSelectionModel>>
{
    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the admissionid parameter.</param>
    /// <returns>The requests for the admission, most recent first.</returns>
    public async Task<List<RequestSelectionModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var admissionId = queryFilters.GetIntegerValue("admissionid");

        var sql = $"""
            {RequestSelectionSql.Projection}
            WHERE r.AdmissionId = @AdmissionId
            {RequestSelectionSql.OrderBy}
            """;

        var result = await connect.QueryAsync<RequestSelectionModel>(sql, new { AdmissionId = admissionId });

        return result.ToList();
    }
}
