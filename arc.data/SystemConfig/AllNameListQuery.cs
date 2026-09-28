using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.SystemConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class AllNameListQuery : IQueryReturningType<List<NameList>>
    {
        public async Task<List<NameList>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"select id, name from namelist order by id";

            var result = await connect.QueryAsync<NameList>(sql);

            return result.ToList();
        }
    }
}
