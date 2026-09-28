using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Returns the patient context for the specimen owning a specimen tag, keyed on the tag id. The join is left
/// outer so a tag whose specimen has been removed still yields a row rather than failing.
/// </summary>
internal class SpecimenPatientBySpecimenTagIdQuery : IQueryReturningType<SpecimenPatientModel>
{
    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the id parameter.</param>
    /// <returns>The patient context for the tag, or a model with a zero specimen id when the tag does not exist.</returns>
    public async Task<SpecimenPatientModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");

        var sql = """
            select coalesce(s.id, st.specimenid, 0) as SpecimenId,
                   coalesce(s.patientid, 0) as patientid,
                   coalesce(s.accessionnumber, '') as accessionnumber
            from specimentag st
            left join specimen s on s.id = st.specimenid
            where st.id = @id
            """;

        var result = await connect.QueryFirstOrDefaultAsync<SpecimenPatientModel>(sql, new { id });

        return result ?? new SpecimenPatientModel { SpecimenId = 0 };
    }
}
