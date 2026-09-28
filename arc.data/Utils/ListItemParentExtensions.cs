using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Utils;

/// <summary>
/// Extension methods for resolving list item parent relationships from listitemparentchild.
/// </summary>
public static class ListItemParentExtensions
{
    /// <summary>
    /// Retrieves the parent list item id for a given child list item.
    /// Used when list items have a hierarchy (e.g. Growth/No Growth parents for SpecimenGrowth).
    /// </summary>
    /// <param name="connect">An open NpgsqlConnection for the query.</param>
    /// <param name="childId">The child list item id.</param>
    /// <returns>The parent list item id, or null if the child has no parent or does not exist.</returns>
    public static async Task<int?> GetParentListItemIdAsync(this NpgsqlConnection connect, int childId)
    {
        const string sql = "select parentid from listitemparentchild where childid = @ChildId limit 1";
        return await connect.QueryFirstOrDefaultAsync<int?>(sql, new { ChildId = childId });
    }
}
