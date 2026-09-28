using arc.data.model.Configuration;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class ConfigListQuery : IQueryReturningType<List<ConfigsDataModel>>
    {
        public async Task<List<ConfigsDataModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"select Id, ConfigName, ConfigTypeId, contents from Configs  order by ConfigName";

            var result = await connect.QueryAsync<ConfigsDataModel>(sql);

            return result.ToList();
        }
    }
}
