using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Security
{
    internal class GetLaboratoriesForListUnfilteredQuery : IQueryReturningType<List<OptionsConfig>>
    {
        public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"select id as key, laboratoryname As text from laboratory order by laboratoryname";
            var result = await connect.QueryAsync<OptionsConfig>(sql);
            return result.ToList();
        }
    }
}
