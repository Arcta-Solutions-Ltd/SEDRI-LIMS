using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    /// <summary>
    /// Query that returns export schedules for a given export profile, including last run date.
    /// Used by the embedded schedules list on the export profile record view.
    /// </summary>
    internal class ExportScheduleListByProfileIdQuery : IQueryReturningType<List<ExportScheduleListModel>>
    {
        /// <inheritdoc />
        public async Task<List<ExportScheduleListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var idParam = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("ExportProfileId", System.StringComparison.OrdinalIgnoreCase))
                ?? queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("id", System.StringComparison.OrdinalIgnoreCase));
            if (idParam == null || string.IsNullOrEmpty(idParam.Value))
            {
                return new List<ExportScheduleListModel>();
            }

            var profileId = int.Parse(idParam.Value);
            var sql = @"
                SELECT es.id AS ""Id"", es.exportprofileid AS ""ExportProfileId"", es.name AS ""Name"",
                    li.value AS ""Frequency"",
                    es.timeofday::text AS ""TimeOfDay"",
                    es.dayofmonth AS ""DayOfMonth"",
                    CASE WHEN es.enabled THEN 'Yes' ELSE 'No' END AS ""Enabled"",
                    (SELECT MAX(erh.runat) FROM exportrunhistory erh WHERE erh.exportscheduleid = es.id) AS ""LastRunAt""
                FROM exportschedule es
                LEFT JOIN listitem li ON li.listid = 139 AND li.id::text = es.frequency
                WHERE es.exportprofileid = @ProfileId
                ORDER BY es.name";

            var results = await connect.QueryAsync<ExportScheduleListModel>(sql, new { ProfileId = profileId });
            return results.ToList();
        }
    }
}
