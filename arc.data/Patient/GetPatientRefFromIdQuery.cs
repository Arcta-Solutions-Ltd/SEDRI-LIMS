using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Patient
{
    internal class GetPatientRefFromIdQuery : IQueryReturningString
    {
        public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            var sql = @"select s.patientref As PatientRef from patient s where s.id = @Id;";

            return await connect.QueryFirstAsync<string>(sql, new { Id = int.Parse(id.Value) });
        }
    }
}
