using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class OrganismCultureCountQuery : IQueryReturningInteger
    {
        async Task<int> IQueryReturningInteger.ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var organismId = queryFilters.Parameters.First(p => p.Key.ToLower() == "id");

            var sql = @"select count(c.Id) from culture c
                    inner join organism o on c.specimenorganismid = o.id and additionalid is not null
                    where specimenorganismid = @OrganismId";

            var result = await connect.QueryFirstAsync<int>(sql, new { OrganismId = int.Parse(organismId.Value) });
            return result;

        }
    }
}
