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
    /// Query that returns all enabled export schedules for the background service.
    /// </summary>
    internal class GetEnabledExportSchedulesQuery : IQueryReturningType<List<ExportScheduleModel>>
    {
        /// <inheritdoc />
        public async Task<List<ExportScheduleModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var sql = @"SELECT id, exportprofileid, name, filter, frequency,
                timeofday AS ""TimeOfDay"", dayofmonth AS ""DayOfMonth"", incrementalonly, enabled,
                modifieddate, outputdirectory, changestoinclude
                FROM exportschedule WHERE enabled = true ORDER BY exportprofileid, name";

            var results = await connect.QueryAsync<ExportScheduleModel>(sql);
            return results.ToList();
        }
    }
}
