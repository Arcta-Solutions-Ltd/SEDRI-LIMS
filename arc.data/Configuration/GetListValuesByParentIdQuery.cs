using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// Query to retrieve the child list items of one parent list item.
    /// </summary>
    /// <remarks>
    /// Hierarchical lists model the parent relation in <c>listitemparentchild</c> rather than by list
    /// membership, so a workflow's StatesList is a list item id and its states are that item's children.
    /// Reading them this way is what lets two workflows share a states list without either of them
    /// hardcoding the underlying list id.
    /// </remarks>
    internal class GetListValuesByParentIdQuery : IQueryReturningType<List<OptionsConfig>>
    {
        /// <summary>
        /// Executes the query and returns the children of the given list item.
        /// </summary>
        /// <param name="connect">Open Npgsql database connection.</param>
        /// <param name="queryFilters">Filter config; expects a "parentid" parameter holding the parent list item id.</param>
        /// <returns>The child items as options, keyed by list item id and ordered for display.</returns>
        public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var parentId = queryFilters.GetIntegerValue("parentid");

            var sql = @"select
                            li.id as key,
                            li.value as text,
                            pc.parentid::text as parentkey
                        from listitemparentchild pc
                        inner join listitem li on li.id = pc.childid
                        inner join list l on l.id = li.listid
                        where pc.parentid = @ParentId
                          and li.enabled = true
                          and li.deleted = false
                          and l.deleted = false
                        order by li.displayorder, li.value";

            var result = await connect.QueryAsync<OptionsConfig>(sql, new { ParentId = parentId });

            return result.ToList();
        }
    }
}
