using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class OrderListQuery : IQueryReturningType<List<OptionsConfig>>
    {
        public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"select Id As Key, Name As Text from OrderCat order by Name";

            var result = await connect.QueryAsync<OptionsConfig>(sql);

            return result.ToList();
        }
    }
}
