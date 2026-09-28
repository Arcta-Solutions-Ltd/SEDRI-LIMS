using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration;

/// <summary>
/// Query class for retrieving list values based on filter configurations.
/// </summary>
internal class GetListValuesQuery : IQueryReturningType<List<OptionsConfig>>
{
    /// <summary>
    /// Executes the query to fetch list values from the database based on provided filters.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="queryFilters">Filter configuration parameters.</param>
    /// <returns>A list of options configurations.</returns>
    public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var listName = queryFilters.Parameters[0].Value.ToLower();
        var includeFixed = bool.Parse(queryFilters.Parameters[1].Value);
        var parentNodesOnly = queryFilters.Parameters.Count > 2 && bool.Parse(queryFilters.Parameters[2].Value);

        var sql = @"select
                            li.id as key,
                            li.value as text,
                            string_agg(distinct pc.parentid::text, ',') as parentkey,
                            li.displayorder
                        from ListItem li
                        inner join List l on l.Id = li.ListId
                        left join listitemparentchild pc on pc.childid = li.id
                        Where LOWER(l.name) = @listName and li.enabled = true and li.deleted = false and l.deleted = false";

        if (parentNodesOnly)
        {
            sql += " and l.internalhierarchy = true and li.fixed = true";
        }
        else if (!includeFixed)
        {
            sql += " and li.fixed = false";
        }

        var orderByClause = listName == "coding"
            ? " Order by LOWER(li.value)"
            : " Order by li.DisplayOrder";
        sql += " group by li.id, li.value, li.displayorder" + orderByClause;

        var result = await connect.QueryAsync<OptionsConfig>(sql, new { listName });

        return result.ToList();
    }
}
