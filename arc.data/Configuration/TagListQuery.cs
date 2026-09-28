using arc.common.Models.Lists;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration;

/// <summary>
/// Query that retrieves the list of tags (ListItem where ListId=105) for the tags list view.
/// </summary>
internal class TagListQuery : IQueryReturningType<List<TagListModel>>
{
    /// <summary>
    /// Executes the query to fetch all tags from the database.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="queryFilters">Filter configuration (search text supported via 'searchText' parameter).</param>
    /// <returns>A list of tag models with Id and Value.</returns>
    public async Task<List<TagListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var searchText = queryFilters.TryGetStringValue("searchtext", out var search) ? search?.ToLower() ?? "" : "";
        var wildcard = string.IsNullOrWhiteSpace(searchText) ? "%" : $"%{searchText}%";

        var sql = """
            select li.id as "Id", li.value as "Value",
                   (select max(pc.parentid) from listitemparentchild pc where pc.childid = li.id) as "ParentTagId"
            from listitem li
            where li.listid = 105 and li.enabled = true and li.deleted = false
            """;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            sql += " and lower(li.value) like @wildcard";
        }

        sql += " order by li.displayorder, li.value";

        var result = await connect.QueryAsync<TagListModel>(sql, new { wildcard });
        return result.ToList();
    }
}
