using arc.app.Common;
using arc.common.Models.Export;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    /// <summary>
    /// Command that upserts the single export profile mapping row for a given profile.
    /// Each export profile has at most one mapping; the unique constraint on
    /// exportprofilemapping.exportprofileid keeps insertions idempotent.
    /// </summary>
    internal class AddExportProfileMappingCommand : ICommandWithTypeReturningInteger<ExportProfileMappingModel>
    {
        /// <inheritdoc />
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExportProfileMappingModel command, ILogWriter logWriter = null)
        {
            var sql = @"INSERT INTO exportprofilemapping (exportprofileid, format, structure, modifieddate)
                VALUES (@ExportProfileId, @Format, @Structure::jsonb, now())
                ON CONFLICT (exportprofileid) DO UPDATE SET
                    format = EXCLUDED.format,
                    structure = EXCLUDED.structure,
                    modifieddate = now()
                RETURNING id";

            var id = await connect.QueryFirstAsync(sql, new
            {
                command.ExportProfileId,
                command.Format,
                command.Structure
            });

            logWriter?.LogInfo($"Export profile mapping upserted: id={id.id}, profileId={command.ExportProfileId}, format={command.Format}", nameof(AddExportProfileMappingCommand), nameof(ExecuteAsync));
            return (int)id.id;
        }
    }
}
