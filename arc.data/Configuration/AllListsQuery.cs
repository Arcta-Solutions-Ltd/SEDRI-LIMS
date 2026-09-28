//using arc.domain.Coding;
//using arc.domain.Configuration.QueryFiltersConfig;
//using Dapper;
//using Npgsql;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace arc.data.Configuration
//{
//    internal class AllListsQuery : IQueryReturningType<List<ListModel>>
//    {
//        public async Task<List<ListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
//        {
//            var sql = @"select Id, Name, Grouping, ParentId, Common, Description from List order by id";

//            var result = await connect.QueryAsync<ListModel>(sql);

//            return result.ToList();
//        }
//    }
//}
