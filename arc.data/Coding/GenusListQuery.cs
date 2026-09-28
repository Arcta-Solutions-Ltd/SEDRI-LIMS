using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class GenusListQuery : IQueryReturningType<List<OptionsConfig>>
    {
        public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {

            if (!queryFilters.Parameters.Any(p => p.Key.ToLower() == "familyid"))
            {
                var sql = @"select Id As Key, Name As Text from Genus order by Name";

                var result = await connect.QueryAsync<OptionsConfig>(sql);

                return result.ToList();
            }
            else
            {
                var familyId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "familyid").First();

                var sql = @"select Id As Key, Name As Text from Genus Where FamilyId = @FamilyId order by Name";

                var result = await connect.QueryAsync<OptionsConfig>(sql, new { FamilyId = int.Parse(familyId.Value) });

                return result.ToList();
            }
        }
    }
}
