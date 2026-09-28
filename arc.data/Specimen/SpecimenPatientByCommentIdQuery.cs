using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Returns the patient context for the specimen owning a specimen comment, keyed on the comment id.
/// </summary>
internal class SpecimenPatientByCommentIdQuery : IQueryReturningType<SpecimenPatientModel>
{
    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the id parameter (must be <c>specimencomment.id</c>, not culture or specimen id).</param>
    /// <returns>The patient context for the specimen owning the comment, or an empty model when not found.</returns>
    public async Task<SpecimenPatientModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");

        var sql = """
            select s.id as SpecimenId, s.patientid, s.accessionnumber
            from specimen s
            inner join specimencomment c on s.id = c.specimenid
            where c.id = @id
            """;

        var result = await connect.QueryFirstOrDefaultAsync<SpecimenPatientModel>(sql, new { id });
        return result ?? new SpecimenPatientModel { SpecimenId = 0, PatientId = 0, StateId = 0 };
    }
}
