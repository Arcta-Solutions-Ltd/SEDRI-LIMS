using arc.common.Models.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Alert;

/// <summary>
/// Data query that loads test criteria (alerttestlines) for a given alert.
/// </summary>
internal class AlertTestCriteriaListByAlertIdQuery : IQueryReturningType<List<AlertTestCriteriaModel>>
{
    public async Task<List<AlertTestCriteriaModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var alertId = queryFilters.GetIntegerValue("AlertId");

        var sql = """
            select
                Id,
                TestName,
                FieldName,
                Comparison,
                CompValue
            from alerttestlines
            where alertid = @alertId
            order by id
            """;

        var results = await connect.QueryAsync<AlertTestCriteriaModel>(sql, new { alertId });
        return results.ToList();
    }
}
