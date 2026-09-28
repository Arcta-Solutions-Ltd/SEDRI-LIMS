using arc.common.Models.Admissions;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Admission;

/// <summary>
/// Returns the admissions held for a patient, most recent first, for the admission selection screen.
/// The admission date and time have no dedicated columns so they are read out of the MoreData blob.
/// </summary>
internal class AdmissionsForPatientQuery : IQueryReturningType<List<AdmissionSelectionModel>>
{
    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the patientid parameter.</param>
    /// <returns>The admissions for the patient ordered by admission date then creation, most recent first.</returns>
    public async Task<List<AdmissionSelectionModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var patientId = queryFilters.GetIntegerValue("patientid");

        var sql = """
            SELECT a.Id,
                   a.PatientId,
                   a.MoreData::jsonb->>'DateOfAdmission' AS AdmissionDate,
                   a.MoreData::jsonb->>'TimeOfAdmission' AS AdmissionTime,
                   (SELECT COUNT(*) FROM Request r WHERE r.AdmissionId = a.Id) AS RequestCount,
                   (SELECT COUNT(*) FROM Specimen s WHERE s.AdmissionId = a.Id) AS SpecimenCount,
                   a.LastModifiedDate
            FROM Admission a
            WHERE a.PatientId = @PatientId
            ORDER BY NULLIF(a.MoreData::jsonb->>'DateOfAdmission', '') DESC NULLS LAST, a.Id DESC
            """;

        var result = await connect.QueryAsync<AdmissionSelectionModel>(sql, new { PatientId = patientId });

        return result.ToList();
    }
}
