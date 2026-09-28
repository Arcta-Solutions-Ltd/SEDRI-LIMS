using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Returns the patient and state context for a specimen, keyed on the specimen id.
/// </summary>
internal class SpecimenPatientByIdQuery : IQueryReturningType<SpecimenPatientModel>
{
    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the id parameter.</param>
    /// <returns>The patient and state context for the specimen.</returns>
    public async Task<SpecimenPatientModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");

        var sql = "select id as SpecimenId, patientid, accessionnumber, stateid from specimen where id = @id";

        return await connect.QueryFirstAsync<SpecimenPatientModel>(sql, new { id });
    }
}
