using arc.common.Models.Coding;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Utils
{
    internal class GetHierarchyFromOrganismId : IQueryReturningType<OrganismHierarchyModel>
    {
        public async Task<OrganismHierarchyModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = int.Parse(queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value);

            var sql = "select * from Organism where id = @OrganismId";
            var result = await connect.QueryFirstAsync<OrganismHierarchyModel>(sql, new { OrganismId = id });

            if (result.GenusId > 0)
            {
                sql = "select familyId as Id from genus where id = @genusId";
                var familyId = await connect.QuerySingleAsync<int>(sql, new { result.GenusId });
                result.FamilyId = familyId;

                sql = "select orderId as id from family where id = @familyId";
                var orderId = await connect.QuerySingleAsync<int>(sql, new { result.FamilyId });
                result.OrderId = orderId;
            }

            return result;
        }
    }
}
