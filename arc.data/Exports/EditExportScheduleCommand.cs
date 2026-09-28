using arc.app.Common;
using arc.common.Models.Export;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    /// <summary>
    /// Command that updates an existing export schedule.
    /// </summary>
    internal class EditExportScheduleCommand : ICommandWithTypeReturningInteger<ExportScheduleModel>
    {
        /// <inheritdoc />
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExportScheduleModel command, ILogWriter logWriter = null)
        {
            var sql = @"UPDATE exportschedule SET
                name = @Name,
                filter = @Filter::jsonb,
                frequency = @Frequency,
                timeofday = @TimeOfDay,
                dayofmonth = @DayOfMonth,
                incrementalonly = @IncrementalOnly,
                enabled = @Enabled,
                outputdirectory = @OutputDirectory,
                changestoinclude = @ChangesToInclude,
                modifieddate = now()
                WHERE id = @Id";

            await connect.ExecuteAsync(sql, new
            {
                command.Id,
                command.Name,
                command.Filter,
                command.Frequency,
                TimeOfDay = command.TimeOfDay,
                command.DayOfMonth,
                command.IncrementalOnly,
                command.Enabled,
                command.OutputDirectory,
                command.ChangesToInclude
            });

            logWriter?.LogInfo($"Export schedule updated: id={command.Id}, name={command.Name}", nameof(EditExportScheduleCommand), nameof(ExecuteAsync));
            return command.Id;
        }
    }
}
