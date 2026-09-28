//using arc.domain.Coding;
//using arc.domain.Configuration.QueryFiltersConfig;
//using Dapper;
//using Npgsql;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace arc.data.Configuration
//{
//    internal class AllListItemsQuery : IQueryReturningType<List<ListItem>>
//    {
//        public async Task<List<ListItem>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
//        {
//            var sql = @"select id,listid, value, parentid, fixed, enabled, displayorder, deleted from listitem order by id";

//            var result = await connect.QueryAsync<ListItem>(sql);

//            return result.ToList();
//        }
//    }
//}
