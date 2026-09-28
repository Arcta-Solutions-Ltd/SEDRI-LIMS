using arc.common.Models.Lists;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// Represents a query to retrieve a list item by value.
    /// </summary>
    internal class ListItemByValueQuery : IQueryReturningType<ListItemModel>
    {
        /// <summary>
        /// Executes the query asynchronously to retrieve a list item based on the specified query filters.
        /// </summary>
        /// <param name="connect">The Npgsql connection to use for the query.</param>
        /// <param name="queryFilters">The query filter configuration.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list item.</returns>
        public async Task<ListItemModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var value = queryFilters.GetStringValue("value");
            var name = queryFilters.GetStringValue("name");

            string sql = string.IsNullOrEmpty(name)
                ? @"SELECT
                        li.id,
                        li.listid,
                        li.value,
                        li.fixed,
                        CASE WHEN li.enabled THEN 'Yes' ELSE 'No' END AS enabled,
                        coalesce(string_agg(distinct pc.parentid::text, ','), '') as parentid
                   FROM listitem li
                   LEFT JOIN listitemparentchild pc ON pc.childid = li.id
                   WHERE Lower(li.value) = Lower(@value)
                   GROUP BY li.id, li.listid, li.value, li.fixed, li.enabled"
                : @"SELECT
                        li.id,
                        li.listid,
                        li.value,
                        li.fixed,
                        CASE WHEN li.enabled THEN 'Yes' ELSE 'No' END AS enabled,
                        coalesce(string_agg(distinct pc.parentid::text, ','), '') as parentid
                   FROM listitem li
                   INNER JOIN list l ON l.id = li.listid
                   LEFT JOIN listitemparentchild pc ON pc.childid = li.id
                   WHERE Lower(li.value) = Lower(@value)
                     AND Lower(l.name) = Lower(@name)
                   GROUP BY li.id, li.listid, li.value, li.fixed, li.enabled";

            return await connect.QueryFirstAsync<ListItemModel>(sql, new { value, name });
        }
    }

}
