using arc.common.Models.Lists;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Configuration;

/// <summary>
/// Query that retrieves a single tag by Id for edit/delete forms.
/// </summary>
internal class SingleTagForTagListQuery : IQueryReturningType<TagListModel>
{
    /// <summary>
    /// Executes the query to fetch a single tag by its Id.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="queryFilters">Filter configuration; expects 'id' parameter.</param>
    /// <returns>The tag model if found, or null.</returns>
    public async Task<TagListModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.TryParseIntegerValue("id", out var tagId) ? tagId : 0;

        var sql = """
            select li.id as "Id", li.value as "Value",
                   (select max(pc.parentid) from listitemparentchild pc where pc.childid = li.id) as "ParentTagId",
                   li.enabled as "Enabled"
            from listitem li
            where li.id = @id and li.listid = 105 and li.deleted = false
            """;

        return await connect.QueryFirstOrDefaultAsync<TagListModel>(sql, new { id });
    }
}
