using arc.common.Models.AST;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Returns <c>antibiotic.id</c> and group listitem id (<c>antibioticcoding.codingid</c>) for the given antibiotics.
/// An antibiotic may appear in multiple groups (multiple rows).
/// </summary>
internal class AntibioticGroupIdByAntibioticIdsQuery : IQueryReturningType<List<AntibioticIdGroupPair>>
{
    public async Task<List<AntibioticIdGroupPair>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        if (!queryFilters.TryGetStringValue("antibioticids", out var raw) || string.IsNullOrWhiteSpace(raw))
        {
            return [];
        }

        var ids = raw.Split(',').Select(s => s.Trim()).Where(s => int.TryParse(s, out _)).Select(int.Parse).Distinct().ToArray();
        if (ids.Length == 0)
        {
            return [];
        }

        const string sql = @"
SELECT ac.antibioticid AS Id, ac.codingid AS GroupId
FROM antibioticcoding ac
WHERE ac.antibioticid = ANY(@Ids)
ORDER BY ac.antibioticid, ac.codingid";

        var rows = await connect.QueryAsync<AntibioticIdGroupPair>(sql, new { Ids = ids });
        return rows.ToList();
    }
}
