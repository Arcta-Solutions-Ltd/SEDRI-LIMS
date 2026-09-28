using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Utils;

/// <summary>
/// Recursively retrieves a tag's ID and all of its descendant tag IDs from the listitemparentchild table.
/// </summary>
internal class TagIdListFromTagIdQuery
{
    /// <summary>
    /// Executes a recursive query to build a list containing the specified tagId
    /// and all child tag IDs found in the listitemparentchild table.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> against which the recursive query will run.
    /// </param>
    /// <param name="tagId">
    /// The root tag ID from which to begin collecting descendant IDs.
    /// </param>
    /// <returns>
    /// A <see cref="Task{List}"/> containing the original <paramref name="tagId"/>
    /// plus any IDs of child tags (and their children) discovered during recursion.
    /// </returns>
    public async Task<List<int>> ExecuteAsync(NpgsqlConnection connect, int tagId)
    {
        var returnIds = new List<int> { tagId };

        var sql = @"select childid from listitemparentchild where parentid = @tagId";

        var results = await connect.QueryAsync<int>(sql, new { tagId });

        if (results.Any())
        {
            foreach (var result in results)
            {
                var newIds = await new TagIdListFromTagIdQuery().ExecuteAsync(connect, result);
                returnIds.AddRange(newIds);
            }
        }

        return returnIds.Distinct().ToList();
    }
}
