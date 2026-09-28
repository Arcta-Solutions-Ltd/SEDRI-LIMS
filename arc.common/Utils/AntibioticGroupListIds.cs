using System.Collections.Generic;
using System.Linq;

namespace arc.common.Utils;

/// <summary>
/// Stable listitem ids for the AntibioticGroup list (list id <see cref="AntibioticGroupListId"/>).
/// </summary>
public static class AntibioticGroupListIds
{
    /// <summary>List id for antibiotic group dropdown/combobox options.</summary>
    public const int AntibioticGroupListId = 82;

    /// <summary>
    /// Listitem id for the "Master" sentinel — means all antibiotic groups, not a row in <c>antibioticgroup</c>.
    /// </summary>
    public const int MasterListItemId = 21;

    /// <summary>
    /// Parses comma-separated stored ids from <c>laboratory.antibioticgroupids</c> or similar fields.
    /// </summary>
    public static List<int> ParseStoredIds(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return [];
        }

        return raw
            .Split(',')
            .Select(s => s.Trim())
            .Where(s => int.TryParse(s, out _))
            .Select(int.Parse)
            .Distinct()
            .ToList();
    }

    /// <summary>
    /// Whether the raw stored value includes the Master (all groups) listitem id.
    /// </summary>
    public static bool IncludesMaster(string raw) =>
        ParseStoredIds(raw).Contains(MasterListItemId);
}
