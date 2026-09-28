using arc.common.Models.Config;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class GetMappingListViewQuery : IQueryReturningType<List<MappingListViewModel>>
    {
        public async Task<List<MappingListViewModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"select Id, ConfigName, contents::jsonb->>'Name' as Name from Configs
                        where ConfigTypeId in ('22')
                        order by ConfigName";

            var result = await connect.QueryAsync<MappingListViewModel>(sql);

            return result.ToList();
        }
    }
}
