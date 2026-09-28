using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Quality.Queries
{
    internal class IqcTestProfileNamesListQuery : IQueryReturningType<List<OptionsConfig>>
    {
        public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"select itp.id as key, concat(itp.name, ' (', li.value, ')') as text from iqctestprofiles itp
                        left join listitem li on li.id = itp.testmethodlistitemid
                        where deleteddate is null order by text;";
            var result = await connect.QueryAsync<OptionsConfig>(sql);
            return result.ToList();
        }
    }
}
