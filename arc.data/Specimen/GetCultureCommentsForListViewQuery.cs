//using arc.common.Models.Specimen;
//using arc.domain.Configuration.ListsConfig;
//using arc.domain.Configuration.QueryFiltersConfig;
//using Dapper;
//using Newtonsoft.Json;
//using Npgsql;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace arc.data.Specimen
//{
//    internal class GetCultureCommentsForListViewQuery : IQueryReturningString
//    {
//        public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
//        {

//            var sql = @"select case when sp.comment is null then li.value else sp.comment end as comment, sp.id, sp.displayonreport, sp.lastmodifieddate, sp.addedby, lj.value as commenttype from specimencomment sp
//                        left outer join listitem li on li.id = sp.cannedcommentid
//                        left outer join listitem lj on lj.id = sp.commenttypeid
//                        where sp.cultureid = @CultureId order by sp.lastmodifieddate";

//            var result = await connect.QueryAsync(sql, new { CultureId = int.Parse(queryFilters.Parameters[0].Value) });

//            return JsonConvert.SerializeObject(result);
//        }
//    }
//}
