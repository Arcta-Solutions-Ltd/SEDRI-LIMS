using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    /// <summary>
    /// Query that returns the last run date for a given export schedule.
    /// Used for incremental export date range calculation.
    /// </summary>
    internal class GetLastRunForScheduleQuery : IQueryReturningType<DateTime?>
    {
        /// <inheritdoc />
        public async Task<DateTime?> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var idParam = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("ExportScheduleId", System.StringComparison.OrdinalIgnoreCase))
                ?? queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("id", System.StringComparison.OrdinalIgnoreCase));
            if (idParam == null || string.IsNullOrEmpty(idParam.Value))
            {
                return null;
            }

            var scheduleId = int.Parse(idParam.Value);
            var sql = @"
                SELECT MAX(runat) FROM exportrunhistory
                WHERE exportscheduleid = @ScheduleId";

            var result = await connect.QueryFirstOrDefaultAsync<DateTime?>(sql, new { ScheduleId = scheduleId });
            return result;
        }
    }
}
