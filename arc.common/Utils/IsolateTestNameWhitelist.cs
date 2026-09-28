using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.common.Utils;

/// <summary>
/// Intersects applicable isolate test names with an optional laboratory comma-separated whitelist.
/// </summary>
public static class IsolateTestNameWhitelist
{
    /// <summary>
    /// Parses a comma-separated list of isolate test form names (trimmed, non-empty segments).
    /// </summary>
    public static List<string> ParseCommaSeparatedNames(string commaSeparated)
    {
        var list = new List<string>();
        if (string.IsNullOrWhiteSpace(commaSeparated))
            return list;
        foreach (var part in commaSeparated.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            var t = part.Trim();
            if (t.Length > 0)
                list.Add(t);
        }
        return list;
    }

    /// <summary>
    /// When <paramref name="commaSeparatedWhitelist"/> is null or whitespace, returns <paramref name="baseList"/> unchanged.
    /// When the whitelist parses to no tokens, returns <paramref name="baseList"/> unchanged.
    /// Otherwise returns only entries from <paramref name="baseList"/> that appear in the whitelist (case-insensitive, trimmed).
    /// </summary>
    /// <param name="baseList">Resolved isolate test names from culture type / organism scope; may be null.</param>
    /// <param name="commaSeparatedWhitelist">Comma-separated test names from laboratory configuration.</param>
    public static IReadOnlyList<string> IntersectWithLaboratorySelection(
        IReadOnlyList<string> baseList,
        string commaSeparatedWhitelist)
    {
        if (baseList == null)
            return null;

        if (baseList.Count == 0)
            return baseList;

        if (string.IsNullOrWhiteSpace(commaSeparatedWhitelist))
            return baseList;

        var whitelist = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var part in commaSeparatedWhitelist.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            var t = part.Trim();
            if (t.Length > 0)
                whitelist.Add(t);
        }

        if (whitelist.Count == 0)
            return baseList;

        return baseList.Where(n => n != null && whitelist.Contains(n.Trim())).ToList();
    }
}
