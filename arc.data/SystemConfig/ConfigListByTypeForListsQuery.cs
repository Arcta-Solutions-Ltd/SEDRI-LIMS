using arc.common.Models.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class ConfigListByTypeForListsQuery : IQueryReturningType<List<ConfigsModel>>
    {
        public async Task<List<ConfigsModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var configTypeId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "configtypeid").First();

            var sql = @"select c.Id, c.ConfigName, c.ConfigTypeId, ct.Type, c.contents from Configs c
                        inner join ConfigType ct on c.ConfigTypeId = ct.Id
                        where ConfigTypeId in (" + configTypeId.Value + @")
                        order by ConfigName";

            var result = await connect.QueryAsync<ConfigsModel>(sql);

            return result.ToList();
        }
    }
}
