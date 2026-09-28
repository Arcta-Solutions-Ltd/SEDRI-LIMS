using arc.app.Common;
using arc.common.Models.Instruments;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Inserts link rows for instrument result source files. Duplicate (instrumentresultid, fileattachmentid) pairs are skipped via ON CONFLICT DO NOTHING.
/// </summary>
internal class AddInstrumentResultFileAttachmentsCommand : ICommandWithTypeReturningInteger<AddInstrumentResultFileAttachmentsModel>
{
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, AddInstrumentResultFileAttachmentsModel command, ILogWriter logWriter = null)
    {
        var ids = command.FileAttachmentIds?.Where(i => i > 0).Distinct().ToArray() ?? System.Array.Empty<int>();
        if (command.InstrumentResultId <= 0 || ids.Length == 0)
        {
            logWriter?.LogInfo(
                $"AddInstrumentResultFileAttachments skipped (instrumentResultId={command.InstrumentResultId}, requestedCount={ids.Length})",
                nameof(AddInstrumentResultFileAttachmentsCommand),
                nameof(ExecuteAsync));
            return 0;
        }

        const string sql = @"
INSERT INTO instrumentresultfileattachments (instrumentresultid, fileattachmentid)
VALUES (@InstrumentResultId, @FileAttachmentId)
ON CONFLICT (instrumentresultid, fileattachmentid) DO NOTHING";

        var inserted = 0;
        foreach (var fileAttachmentId in ids)
        {
            var rows = await connect.ExecuteAsync(sql, new { command.InstrumentResultId, FileAttachmentId = fileAttachmentId });
            inserted += rows;
        }

        logWriter?.LogInfo(
            $"AddInstrumentResultFileAttachments instrumentResultId={command.InstrumentResultId} requested={ids.Length} inserted={inserted}",
            nameof(AddInstrumentResultFileAttachmentsCommand),
            nameof(ExecuteAsync));

        return inserted;
    }
}
