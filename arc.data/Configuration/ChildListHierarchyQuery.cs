using arc.common.Models.Config;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// Returns all lists that have a parent list defined in the table hierarchy.
    /// </summary>
    internal class ChildListHierarchyQuery : IQueryReturningType<List<ChildListInfoModel>>
    {
        /// <summary>
        /// Loads child lists with their parent list id and name.
        /// </summary>
        /// <param name="connect">Database connection.</param>
        /// <param name="queryFilters">Unused filters.</param>
        /// <returns>Child list hierarchy rows.</returns>
        public async Task<List<ChildListInfoModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"select l1.id as ListId, l1.parentid as ParentListId, l2.name as ParentListName
                    from list l1
                    inner join list l2 on l2.id = l1.parentid
                    where l1.deleted = false and l1.parentid is not null
                    order by l1.id";

            var result = await connect.QueryAsync<ChildListInfoModel>(sql);
            return result.ToList();
        }
    }
}
