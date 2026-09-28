using arc.common.Models.Laboratory;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Loads raw <c>laboratory.antibioticgroupids</c> and resolves listitem IDs to <c>antibioticgroup.id</c> values.
/// </summary>
internal class LaboratoryAntibioticGroupFilterInfoQuery : IQueryReturningType<LaboratoryAntibioticGroupFilterInfo>
{
    public async Task<LaboratoryAntibioticGroupFilterInfo> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        if (!queryFilters.TryParseIntegerValue("id", out var labId) || labId <= 0)
        {
            return null;
        }

        const string sql = @"
SELECT l.antibioticgroupids AS AntibioticGroupIdsRaw,
       COALESCE((
         SELECT ARRAY_AGG(DISTINCT COALESCE(ag_from_li.id, ag_direct.id))
         FROM unnest(coalesce(string_to_array(nullif(trim(l.antibioticgroupids), ''), ','), array[]::text[])) AS aid(rawid)
         LEFT JOIN listitem li ON li.id = nullif(trim(aid.rawid), '')::int
         LEFT JOIN antibioticgroup ag_from_li ON ag_from_li.name = li.value
         LEFT JOIN antibioticgroup ag_direct ON ag_direct.id = nullif(trim(aid.rawid), '')::int
         WHERE COALESCE(ag_from_li.id, ag_direct.id) IS NOT NULL
       ), ARRAY[]::int[]) AS AllowedAntibioticGroupIds
FROM laboratory l
WHERE l.id = @LaboratoryId";

        return await connect.QueryFirstOrDefaultAsync<LaboratoryAntibioticGroupFilterInfo>(sql, new { LaboratoryId = labId });
    }
}
