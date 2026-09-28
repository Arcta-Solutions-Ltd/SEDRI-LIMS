using arc.common.Models.Specimen;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen
{
    internal class GetCommentsForOrderingQuery : IQueryReturningString
    {
        public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"select case when sp.comment is null then li.value else sp.comment end, sp.id, sp.order from specimencomment sp
                        left outer join listitem li on li.id = sp.cannedcommentid
                        where specimenid = @SpecimenId and commenttypeid = 1231 order by order";

            var result = await connect.QueryAsync<string>(sql, new { SpecimenId = int.Parse(queryFilters.Parameters[0].Value) });

            return JsonConvert.SerializeObject(result);
        }

    }
}
