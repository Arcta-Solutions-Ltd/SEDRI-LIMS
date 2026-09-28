using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Tests;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Tests
{
    internal class TestsForCultureQuery : IQueryReturningType<List<CultureTest>>
    {
        public async Task<List<CultureTest>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "cultureid").First();

            var sql = @"select * from CultureTests where CultureId = @Id order by requested, id";

            var result = await connect.QueryAsync<CultureTest>(sql, new { Id = int.Parse(id.Value) });

            return result.ToList();
        }
    }
}
