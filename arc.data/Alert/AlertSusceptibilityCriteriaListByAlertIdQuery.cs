using arc.common.Models.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Alert;

/// <summary>
/// Data query that loads susceptibility criteria (alertlines) for a given alert.
/// Resolves antibiotic IDs (comma-separated) to names and susceptibility ID to display text.
/// </summary>
internal class AlertSusceptibilityCriteriaListByAlertIdQuery : IQueryReturningType<List<AlertSusceptibilityCriteriaModel>>
{
    public async Task<List<AlertSusceptibilityCriteriaModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var alertId = queryFilters.GetIntegerValue("AlertId");

        var sql = """
            select
                al.Id,
                (select string_agg(a.antibioticname, ', ' order by a.id)
                 from (select unnest(string_to_array(al.antibioticid, ',')) as aid) t
                 join antibiotic a on a.id = nullif(trim(t.aid), '')::int
                 where trim(t.aid) ~ '^\d+$') as antibioticnames,
                li.value as susceptibilityname
            from alertlines al
            left join listitem li on li.id = al.susceptibilityid
            where al.alertid = @alertid
            order by al.id
            """;

        var results = await connect.QueryAsync<AlertSusceptibilityCriteriaModel>(sql, new { alertId });
        return results.ToList();
    }
}
