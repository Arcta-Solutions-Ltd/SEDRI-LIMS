using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    internal class ArchiveStateListQuery : IQueryReturningType<List<OptionsConfig>>
    {
        public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"select id as key, value as text from listitem where listid = 60 and (id = 534 or id = 528 or id = 537) and deleted = false order by value";

            var result = await connect.QueryAsync<OptionsConfig>(sql);

            return result.ToList();
        }
    }
}
