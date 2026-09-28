using arc.common.Models.Alert;
using arc.domain.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Alert
{
    /// <summary>
    /// Data query that returns alerts that apply to direct tests (no susceptibility criteria, have test criteria).
    /// </summary>
    internal class DirectTestAlertsQuery : IQueryReturningType<List<AlertDetailsModel>>
    {
        public async Task<List<AlertDetailsModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"with astlines as (select alertid, count(*) as total from alertlines group by alertid),
                        testlines as (select alertid, count(*) as total from alerttestlines group by alertid)
                        select a.Id, a.AlertName, a.SpecificationId, a.AlertMessage, a.SusceptibilityAndOr,
                        a.TestAndOr, a.Enabled, a.DoesExist, a.TagId, a.alerttypeid
                        from alert a
                        left outer join astlines ast on a.Id = ast.alertid
                        left outer join testlines tst on a.Id = tst.alertid
                        where ast.total is null and tst.total > 0 and Enabled = 'Yes' and
                        a.OrderId = 0 and a.FamilyId = 0 and a.OrganismId = 0 and a.OrgGroupCodingId = 0";

            var matchingAlerts = await connect.QueryAsync<AlertDetailsModel>(sql);

            foreach (var singleAlert in matchingAlerts)
            {
                sql = @"select * from alerttestlines where alertid = @AlertId";
                var alertTestLines = await connect.QueryAsync<TestGrid>(sql, new { AlertId = singleAlert.Id });
                singleAlert.TestGrid = alertTestLines.Select((t) => new TestGridModel
                { Test = t.TestName, Field = t.FieldName, Comparison = t.Comparison, ListValue = t.CompValue, StringValue = t.CompValue, NumberValue = t.CompValue }).ToList();
            }

            return matchingAlerts.ToList();
        }

    }
}
