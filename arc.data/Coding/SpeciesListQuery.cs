using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class SpeciesListQuery : IQueryReturningType<List<OptionsConfig>>
    {
        public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {

            if (!queryFilters.Parameters.Any(p => p.Key.ToLower() == "genusid"))
            {
                var sql = @"select Id As Key, Name As Text from Species order by Name";

                var result = await connect.QueryAsync<OptionsConfig>(sql);

                return result.ToList();
            }
            else
            {
                var genusId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "genusid").First();

                var sql = @"select Id As Key, Name As Text from Species Where GenusId = @GenusId order by Name";

                var result = await connect.QueryAsync<OptionsConfig>(sql, new { GenusId = int.Parse(genusId.Value) });

                return result.ToList();
            }
        }
    }
}
