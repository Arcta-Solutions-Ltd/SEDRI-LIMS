using arc.common.Models.AST;
using System.Collections.Generic;
using System.Linq;

namespace arc.common.Utils;

/// <summary>
/// Resolves stored antibiotic group ids to <c>antibioticgroup.id</c> table ids using preloaded query results.
/// </summary>
public static class AntibioticGroupIdResolver
{
    /// <summary>
    /// Builds a lookup from stored id to resolved table id.
    /// </summary>
    /// <param name="pairs">Rows from the antibiotic group table-id resolution query.</param>
    /// <returns>Dictionary keyed by stored id with <c>antibioticgroup.id</c> values.</returns>
    public static IReadOnlyDictionary<int, int> ToLookup(IEnumerable<AntibioticGroupStoredTableIdPair> pairs)
    {
        if (pairs == null)
        {
            return new Dictionary<int, int>();
        }

        return pairs
            .Where(p => p.StoredId > 0 && p.TableId > 0)
            .GroupBy(p => p.StoredId)
            .ToDictionary(g => g.Key, g => g.First().TableId);
    }

    /// <summary>
    /// Resolves a single stored group id to a table id.
    /// When the lookup has no entry, <paramref name="storedGroupId"/> is returned as a legacy fallback
    /// (environments that persist <c>antibioticgroup.id</c> directly in <c>antibioticgroupid</c> columns).
    /// </summary>
    /// <param name="storedGroupId">Stored listitem id or legacy table id.</param>
    /// <param name="lookup">Preloaded stored-to-table map.</param>
    /// <returns>Resolved <c>antibioticgroup.id</c>, or null when <paramref name="storedGroupId"/> is not positive.</returns>
    public static int? ResolveTableId(int storedGroupId, IReadOnlyDictionary<int, int> lookup)
    {
        if (storedGroupId <= 0)
        {
            return null;
        }

        if (lookup != null && lookup.TryGetValue(storedGroupId, out var tableId))
        {
            return tableId;
        }

        return storedGroupId;
    }

    /// <summary>
    /// Resolves a stored group id using only explicit query results (no legacy fallback).
    /// Use for membership and laboratory allow-list resolution where treating an unresolved listitem id
    /// as a table id would yield zero antibiotics.
    /// </summary>
    /// <param name="storedGroupId">Stored listitem id or legacy table id.</param>
    /// <param name="lookup">Preloaded stored-to-table map from <c>AntibioticGroupTableIdsByStoredIdsQuery</c>.</param>
    /// <returns>Resolved <c>antibioticgroup.id</c>, or null when not explicitly resolved.</returns>
    public static int? ResolveTableIdStrict(int storedGroupId, IReadOnlyDictionary<int, int> lookup)
    {
        if (storedGroupId <= 0)
        {
            return null;
        }

        if (lookup != null && lookup.TryGetValue(storedGroupId, out var tableId))
        {
            return tableId;
        }

        return null;
    }
}
