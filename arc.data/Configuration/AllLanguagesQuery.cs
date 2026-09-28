using arc.common.Models.Language;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    internal class AllLanguagesQuery : IQueryReturningType<List<AllLanguageModel>>
    {
        public async Task<List<AllLanguageModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"select id, translationid, sourceid, pack from language order by id";

            var result = await connect.QueryAsync<AllLanguageModel>(sql);

            return result.ToList();
        }
    }
}
