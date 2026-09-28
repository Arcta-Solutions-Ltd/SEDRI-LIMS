using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Patient
{
    /// <summary>
    /// Query to retrieve specimen/patient details from a PatientTag record for workflow and alert handling.
    /// </summary>
    internal class GetSinglePatientTagQuery : IQueryReturningType<SpecimenPatientModel>
    {
        /// <inheritdoc />
        public async Task<SpecimenPatientModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            var sql = @"select coalesce(p.id, pt.patientid, 0) As PatientId, 0 As SpecimenId
                from patienttag pt
                left join patient p on p.id = pt.patientid
                where pt.id = @Id;";

            var result = await connect.QueryFirstOrDefaultAsync<SpecimenPatientModel>(sql, new { Id = int.Parse(id.Value) });
            return result ?? new SpecimenPatientModel { SpecimenId = 0 };
        }
    }
}
