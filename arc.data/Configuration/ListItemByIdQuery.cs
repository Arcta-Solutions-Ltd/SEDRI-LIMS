using arc.common.Models.Lists;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// This class defines the query to retrieve a list item by its ID.
    /// Implements the IQueryReturningType interface for ListItemModel.
    /// </summary>
    internal class ListItemByIdQuery : IQueryReturningType<ListItemModel>
    {
        /// <summary>
        /// Executes the query to retrieve a list item by its ID.
        /// </summary>
        /// <param name="connect">The NpgsqlConnection object for database connection.</param>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the ListItemModel object.</returns>
        public async Task<ListItemModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            var sql = @"select
                            li.id,
                            li.listid,
                            li.value,
                            li.fixed,
                            case when li.enabled then 'Yes' else 'No' end as enabled,
                            coalesce(string_agg(distinct pc.parentid::text, ','), '') as parentid
                        from listItem li
                        left join listitemparentchild pc on pc.childid = li.id
                        Where li.Id = @Id
                        group by li.id, li.listid, li.value, li.fixed, li.enabled";

            var result = await connect.QueryAsync<ListItemModel>(sql, new { Id = int.Parse(id.Value) });

            return result.First();
        }
    }
}
