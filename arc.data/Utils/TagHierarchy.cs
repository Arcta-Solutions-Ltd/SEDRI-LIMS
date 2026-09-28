using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Utils;

/// <summary>
/// Helper for expanding tag IDs to include all descendants in the hierarchy.
/// Used by TagHierarchyListQuery and SpecimenFilter for hierarchical tag filtering.
/// </summary>
internal class TagHierarchy
{
    /// <summary>
    /// Returns a comma-separated string of tag IDs including the specified tag and all its descendants.
    /// </summary>
    /// <param name="connect">An open NpgsqlConnection for hierarchy queries.</param>
    /// <param name="tagId">The root tag ID to expand.</param>
    /// <returns>Comma-separated string of tag IDs (tagId + all descendants).</returns>
    public async Task<string> GetStringList(NpgsqlConnection connect, int tagId)
    {
        var tagQuery = new TagIdListFromTagIdQuery();
        var tagIds = await tagQuery.ExecuteAsync(connect, tagId);
        return string.Join(",", tagIds);
    }
}
