using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Patient
{
    internal class PatientRefDuplicateQuery : IQueryReturningInteger
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var patientRef = queryFilters.Parameters.Where(p => p.Key.ToLower() == "patientref").First();

            var sql = "Select count(*) from patient where patientRef = @PatientRef";

            return await connect.QueryFirstAsync<int>(sql, new { PatientRef = patientRef.Value });
        }
    }
}
