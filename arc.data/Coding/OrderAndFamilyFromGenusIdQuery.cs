using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class OrderAndFamilyFromGenusIdQuery : IQueryReturningType<GenusModel>
    {
        public async Task<GenusModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var genusId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "genusid").First();

            var sql = @"select g.id as genusid, g.familyid, f.orderid from genus g
                        inner join family f on f.id = g.familyid
                        where g.id = @genusId";

            var result = await connect.QueryFirstAsync<GenusModel>(sql, new { GenusId = int.Parse(genusId.Value) });

            return result;
        }
    }
}
