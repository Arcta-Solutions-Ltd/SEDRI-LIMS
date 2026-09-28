using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Security
{
    internal class GetLaboratoriesForListQuery : IQueryReturningType<List<OptionsConfig>>
    {
        public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var labid = queryFilters.Parameters.Where(p => p.Key.ToLower() == "laboratoryid").First();

            var sql = @"select id as key, laboratoryname As text from laboratory order by laboratoryname";

            var result = await connect.QueryAsync<OptionsConfig>(sql);

            if (result.Count() == 1 || !string.IsNullOrWhiteSpace(labid.Value))
            {
                IEnumerable<OptionsConfig> emptyResult = new List<OptionsConfig>() { };
                result = emptyResult;
            }

            return result.ToList();
        }
    }
}
