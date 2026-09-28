using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Tests
{
    internal class CultureTestUsageCountQuery : IQueryReturningInteger
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();
            var idList = id.Value.Split("|");

            var sql = "Select count(*) from culturetests where TestName = @name";

            return await connect.QueryFirstAsync<int>(sql, new { name = idList[1] });
        }
    }
}
