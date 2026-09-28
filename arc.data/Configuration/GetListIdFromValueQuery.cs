using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace arc.data.Configuration;

/// <summary>
/// Represents a query to retrieve the list ID based on value.
/// </summary>
internal class GetListIdFromValueQuery : IQueryReturningInteger
{
    /// <summary>
    /// Executes the query asynchronously to retrieve the list ID based on the specified query filters.
    /// </summary>
    /// <param name="connect">The Npgsql connection to use for the query.</param>
    /// <param name="queryFilters">The query filter configuration.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list ID.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        ArgumentNullException.ThrowIfNull(connect);
        ArgumentNullException.ThrowIfNull(queryFilters);

        var value = queryFilters.GetStringValue("value");
        var parentId = queryFilters.GetIntegerValue("parentid");
        var listId = queryFilters.GetIntegerValue("listid");

        var sql = parentId != 0
            ? @"SELECT li.Id
                 FROM ListItem li
                 INNER JOIN listitemparentchild pc ON pc.childid = li.id
                 WHERE LOWER(li.Value) = LOWER(@value)
                   AND li.ListId = @listId
                   AND pc.ParentId = @parentId"
            : @"SELECT Id
                 FROM ListItem
                 WHERE LOWER(Value) = LOWER(@value)
                   AND ListId = @listId";

        return await connect.QueryFirstOrDefaultAsync<int>(sql, new { value, parentId, listId });
    }
}
