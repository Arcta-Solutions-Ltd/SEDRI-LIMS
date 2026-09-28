using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Utils;

/// <summary>
/// Retrieves a comma-separated string of tag IDs for given tag(s) and all their descendants in the hierarchy.
/// Supports comma-separated tagid parameter for multiple tag selection.
/// </summary>
internal class TagHierarchyListQuery : IQueryReturningString
{
    /// <summary>
    /// Executes the query to build the tag hierarchy list.
    /// Parses the "tagid" filter (comma-separated IDs supported); for each tag,
    /// uses <see cref="TagHierarchy"/> to recursively collect the tag and all descendant IDs.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> against which the query will run.
    /// </param>
    /// <param name="queryFilters">
    /// A <see cref="QueryFilterConfig"/> containing parameters.
    /// Must include a "tagid" parameter (single ID or comma-separated).
    /// </param>
    /// <returns>
    /// A <see cref="Task{String}"/> that resolves to a comma-separated list of IDs,
    /// or an empty string if the "tagid" filter is missing or invalid.
    /// </returns>
    public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var tagIdParam = queryFilters.Parameters?.FirstOrDefault(p => p.Key.Equals("tagid", System.StringComparison.OrdinalIgnoreCase));

        if (tagIdParam == null || string.IsNullOrWhiteSpace(tagIdParam.Value))
        {
            return "";
        }

        var tagHierarchy = new TagHierarchy();
        var allIds = new HashSet<int>();

        foreach (var part in tagIdParam.Value.Split(','))
        {
            if (string.IsNullOrWhiteSpace(part) || !int.TryParse(part.Trim(), out var tagId))
            {
                continue;
            }

            var expanded = await tagHierarchy.GetStringList(connect, tagId);
            if (!string.IsNullOrWhiteSpace(expanded))
            {
                foreach (var idStr in expanded.Split(','))
                {
                    if (int.TryParse(idStr.Trim(), out var id))
                    {
                        allIds.Add(id);
                    }
                }
            }
        }

        return allIds.Count > 0 ? string.Join(",", allIds) : "";
    }
}
