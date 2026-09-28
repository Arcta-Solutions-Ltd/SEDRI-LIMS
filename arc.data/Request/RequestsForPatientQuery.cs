using arc.common.Models.Requests;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Request;

/// <summary>
/// Returns every request held for a patient, most recent first, for the request selection screen.
/// Used by forms whose request selection is scoped to the patient rather than to a single admission.
/// </summary>
internal class RequestsForPatientQuery : IQueryReturningType<List<RequestSelectionModel>>
{
    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the patientid parameter.</param>
    /// <returns>The requests for the patient, most recent first.</returns>
    public async Task<List<RequestSelectionModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var patientId = queryFilters.GetIntegerValue("patientid");

        var sql = $"""
            {RequestSelectionSql.Projection}
            WHERE r.PatientId = @PatientId
            {RequestSelectionSql.OrderBy}
            """;

        var result = await connect.QueryAsync<RequestSelectionModel>(sql, new { PatientId = patientId });

        return result.ToList();
    }
}
