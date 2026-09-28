using arc.app.Common;
using arc.common.Models.Export;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    /// <summary>
    /// Command that inserts a new export schedule.
    /// </summary>
    internal class AddExportScheduleCommand : ICommandWithTypeReturningInteger<ExportScheduleModel>
    {
        /// <inheritdoc />
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExportScheduleModel command, ILogWriter logWriter = null)
        {
            var sql = @"INSERT INTO exportschedule (exportprofileid, name, filter, frequency, timeofday, dayofmonth, incrementalonly, enabled, modifieddate, outputdirectory, changestoinclude)
                VALUES (@ExportProfileId, @Name, @Filter::jsonb, @Frequency, @TimeOfDay, @DayOfMonth, @IncrementalOnly, @Enabled, now(), @OutputDirectory, @ChangesToInclude)
                RETURNING id";

            var id = await connect.QueryFirstAsync(sql, new
            {
                command.ExportProfileId,
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

            logWriter?.LogInfo($"Export schedule created: id={id.id}, name={command.Name}, profileId={command.ExportProfileId}", nameof(AddExportScheduleCommand), nameof(ExecuteAsync));
            return (int)id.id;
        }
    }
}
