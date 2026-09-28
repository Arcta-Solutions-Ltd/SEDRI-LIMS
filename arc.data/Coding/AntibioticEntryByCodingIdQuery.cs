using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class AntibioticEntryByCodingIdQuery : IQueryReturningType<Antibiotic>
    {
        public async Task<Antibiotic> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("id");

            var sql = @"select distinct ant.Id, ant.AntibioticName, ant.Code, ant.GroupId, ant.Atc, ant.Cid, ant.Loinc from Antibiotic ant 
                        inner join AntibioticCoding ac on ac.antibioticid = ant.id
                        where  ac.Id = @Id";

            var result = await connect.QueryFirstAsync<Antibiotic>(sql, new { Id = id });

            return result;
        }
    }
}
