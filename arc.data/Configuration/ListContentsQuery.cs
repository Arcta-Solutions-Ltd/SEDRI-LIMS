using arc.common.Models.Lists;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// This class defines the query to retrieve list contents by list ID.
    /// Implements the IQueryReturningType interface for ListContentsModel.
    /// </summary>
    internal class ListContentsQuery : IQueryReturningType<List<ListContentsModel>>
    {
        /// <summary>
        /// Executes the query to retrieve list contents by list ID.
        /// </summary>
        /// <param name="connect">The NpgsqlConnection object for database connection.</param>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains a list of ListContentsModel objects.</returns>
        public async Task<List<ListContentsModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var listId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "listid").First();

            var searchText = queryFilters.Parameters.Where(p => p.Key.ToLower() == "searchtext");
            var tx = searchText != null && searchText.Count() > 0 ? searchText.First().Value + "%" : "%";
            var orderBy = queryFilters.OrderBy;
            var orderDescending = queryFilters.OrderDescending;
            var orderClause = "";
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                orderClause = " order by " + orderBy + " " + (orderDescending == true ? " desc" : " asc");
            }
            else
            {
                orderClause = @" order by
                            CASE WHEN LOWER(lst.name) = 'coding' THEN LOWER(li1.value) END,
                            CASE WHEN LOWER(lst.name) <> 'coding' THEN li1.displayorder END,
                            li1.value";
            }

            var sql = @"select
                            li1.Id,
                            li1.value,
                            string_agg(distinct li2.value, ',') as parent,
                            Case When li1.Enabled Then 'Yes' Else 'No' end as Enabled
                        from listitem li1
                        inner join list lst on lst.id = li1.listid
                        left outer join listitemparentchild pc on pc.childid = li1.id
                        left outer join listitem li2 on pc.parentid = li2.Id
                        where li1.listid = @ListId
                          and (LOWER(li1.value) ilike @tx or LOWER(li2.value) ilike @tx)
                          and li1.Deleted = false
                        group by li1.Id, li1.value, li1.Enabled, li1.displayorder, lst.name"
                        + orderClause + " limit '500'";

            var result = await connect.QueryAsync<ListContentsModel>(sql, new { ListId = int.Parse(listId.Value), tx = tx });

            return result.ToList();
        }
    }
}
