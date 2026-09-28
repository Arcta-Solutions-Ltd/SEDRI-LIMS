using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class SubSpeciesListQuery : IQueryReturningType<List<OptionsConfig>>
    {
        public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var speciesId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "speciesid").First();

            var sql = @"select Id As Key, Name As Text from SubSpecies Where SpeciesId = @SpeciesId order by Name";

            var result = await connect.QueryAsync<OptionsConfig>(sql, new { SpeciesId = int.Parse(speciesId.Value) });

            return result.ToList();
        }
    }
}
