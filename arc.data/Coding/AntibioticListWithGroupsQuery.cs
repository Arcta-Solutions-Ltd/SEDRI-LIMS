using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class AntibioticListWithGroupsQuery : IQueryReturningType<List<OptionsConfig>>
    {
        public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"with antibioticlist as (
                    select id as key, antibioticname As text, groupid as parentkey from antibiotic 
                    union
                    select id+100000 as key, value as text, 0 as parentkey from listitem where listid = 82 and id != 21
	                    ) 
                    select * from antibioticlist
                    order by text";

            var result = await connect.QueryAsync<OptionsConfig>(sql);

            return result.ToList();
        }
    }
}
