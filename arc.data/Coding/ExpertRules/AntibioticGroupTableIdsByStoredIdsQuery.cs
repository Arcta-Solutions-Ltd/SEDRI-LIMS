using arc.common.Models.AST;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Resolves stored antibiotic group ids (listitem ids from comboboxes or legacy <c>antibioticgroup.id</c> values)
/// to <c>antibioticgroup.id</c> table ids used for membership and laboratory filtering.
/// </summary>
internal class AntibioticGroupTableIdsByStoredIdsQuery : IQueryReturningType<List<AntibioticGroupStoredTableIdPair>>
{
    /// <inheritdoc />
    public async Task<List<AntibioticGroupStoredTableIdPair>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        if (!queryFilters.TryGetStringValue("storedgroupids", out var raw) || string.IsNullOrWhiteSpace(raw))
        {
            return [];
        }

        var ids = raw.Split(',').Select(s => s.Trim()).Where(s => int.TryParse(s, out _)).Select(int.Parse).Distinct().ToArray();
        if (ids.Length == 0)
        {
            return [];
        }

        const string sql = @"
SELECT s.id AS StoredId,
       COALESCE(ag_from_li.id, ag_direct.id) AS TableId
FROM unnest(@StoredIds) AS s(id)
LEFT JOIN listitem li ON li.id = s.id
LEFT JOIN antibioticgroup ag_from_li ON ag_from_li.name = li.value
LEFT JOIN antibioticgroup ag_direct ON ag_direct.id = s.id
WHERE COALESCE(ag_from_li.id, ag_direct.id) IS NOT NULL";

        var rows = await connect.QueryAsync<AntibioticGroupStoredTableIdPair>(sql, new { StoredIds = ids });
        return rows.ToList();
    }
}
