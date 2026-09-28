using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Returns the patient and state context for the specimen owning a culture, keyed on the culture id.
/// </summary>
internal class SpecimenPatientByCultureIdQuery : IQueryReturningType<SpecimenPatientModel>
{
    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the id parameter.</param>
    /// <returns>The patient and state context for the specimen owning the culture.</returns>
    public async Task<SpecimenPatientModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");

        var sql = """
            select s.id as SpecimenId, s.patientid, s.accessionnumber, s.stateid
            from specimen s
            inner join culture c on c.SpecimenId = s.Id
            where c.id = @id
            """;

        return await connect.QueryFirstAsync<SpecimenPatientModel>(sql, new { id });
    }
}
