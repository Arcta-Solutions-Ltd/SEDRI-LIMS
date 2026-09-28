using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Patient
{
    /// <summary>
    /// Loads patient context from an existing <c>patientcomment</c> row by comment id.
    /// </summary>
    internal class GetSingleCommentQuery : IQueryReturningType<SpecimenPatientModel>
    {
        /// <summary>
        /// Returns the patient id for a patient comment record. <paramref name="queryFilters"/> must supply comment id, not patient id.
        /// </summary>
        public async Task<SpecimenPatientModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            var sql = @"select s.id As PatientId from patient s inner join patientcomment c on s.id = c.patientid where c.id = @Id;";

            var result = await connect.QueryFirstOrDefaultAsync<SpecimenPatientModel>(sql, new { Id = int.Parse(id.Value) });
            return result ?? new SpecimenPatientModel { SpecimenId = 0, PatientId = 0, StateId = 0 };
        }
    }
}
