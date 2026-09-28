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
    /// Query to retrieve list values for a specific list (by Id).
    /// Returns each list item as an option with a Key, Text, and a ParentKey that
    /// is a comma-separated list of parent Ids from the listitemparentchild relation.
    /// </summary>
    internal class GetListValuesByIdQuery : IQueryReturningType<List<OptionsConfig>>
    {
        /// <summary>
        /// Executes the query and returns options for a list specified by Id.
        /// </summary>
        /// <param name="connect">Open Npgsql database connection.</param>
        /// <param name="queryFilters">Filter config; expects first parameter to be the list Id.</param>
        /// <returns>List of <see cref="OptionsConfig"/> with Key, Text, and CSV ParentKey.</returns>
        public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters[0].Value;

            var sql = @"select
                            li.id as key,
                            li.value as text,
                            string_agg(distinct pc.parentid::text, ',') as parentkey
                        from ListItem li
                        inner join List l on l.Id = li.ListId
                        left join listitemparentchild pc on pc.childid = li.id
                        Where ListId = @Id and enabled = true and l.deleted = false and li.deleted = false
                        group by li.id, li.value, li.displayorder, l.name
                        Order by
                            CASE WHEN LOWER(l.name) = 'coding' THEN LOWER(li.value) END,
                            CASE WHEN LOWER(l.name) <> 'coding' THEN li.displayorder END,
                            li.value";

            var result = await connect.QueryAsync<OptionsConfig>(sql, new { Id = int.Parse(id) });

            return result.ToList();
        }
    }
}
