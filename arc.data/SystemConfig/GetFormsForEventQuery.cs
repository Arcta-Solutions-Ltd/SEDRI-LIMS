using arc.common.Models.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class GetFormsForEventQuery : IQueryReturningType<List<ConfigsModel>>
    {
        public async Task<List<ConfigsModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var eventName = queryFilters.Parameters.Where(p => p.Key.ToLower() == "eventname").First();

            var sql = @"select * from configs where LOWER(contents->>'saveEvent') = @EventName";

            var result = await connect.QueryAsync<ConfigsModel>(sql, new { EventName = eventName.Value.ToLower() });

            return result.ToList();
        }
    }
}
