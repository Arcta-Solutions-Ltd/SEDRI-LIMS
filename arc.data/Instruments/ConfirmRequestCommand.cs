using arc.app.Common;
using arc.common.Models.Instruments;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Confirms an outbound instrument request by setting the instrument result status to &quot;Requested&quot; (883) and optionally linking uploaded file attachment ids to the same row.
/// </summary>
internal class ConfirmRequestCommand : ICommandWithTypeReturningInteger<RequestConfirmModel>
{
    /// <summary>
    /// Updates <c>instrumentresults.statusid</c> to 883 for the given id, then inserts any <c>SourceFileAttachmentIds</c> into <c>instrumentresultfileattachments</c> for the same row.
    /// Attachment inserts use <c>ON CONFLICT DO NOTHING</c> so duplicate pairs are ignored.
    /// </summary>
    /// <param name="connect">The PostgreSQL database connection.</param>
    /// <param name="requestModel">The instrument result id, optional attachment ids, and instrument name.</param>
    /// <param name="logWriter">The log writer for diagnostic messages.</param>
    /// <returns>Sum of rows affected by the status update and attachment inserts.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, RequestConfirmModel requestModel, ILogWriter logWriter = null)
    {
        if (requestModel.Id <= 0)
        {
            logWriter?.LogInfo(
                $"ConfirmRequest skipped: invalid instrument result id (Id={requestModel.Id})",
                nameof(ConfirmRequestCommand),
                nameof(ExecuteAsync));
            return 0;
        }

        const string updateSql = @"update instrumentresults set statusid = 883 where Id = @Id";
        var updated = await connect.ExecuteAsync(updateSql, requestModel);
        if (updated == 0)
        {
            logWriter?.LogInfo(
                $"ConfirmRequest: no instrument result row updated (Id={requestModel.Id}) — check id and pending status",
                nameof(ConfirmRequestCommand),
                nameof(ExecuteAsync));
            return 0;
        }

        var ids = requestModel.SourceFileAttachmentIds?.Where(i => i > 0).Distinct().ToArray() ?? System.Array.Empty<int>();
        if (ids.Length == 0)
        {
            logWriter?.LogInfo(
                $"ConfirmRequest instrumentResultId={requestModel.Id} status updated (statusid=883), no attachment ids",
                nameof(ConfirmRequestCommand),
                nameof(ExecuteAsync));
            return updated;
        }

        const string insertSql = @"
INSERT INTO instrumentresultfileattachments (instrumentresultid, fileattachmentid)
VALUES (@InstrumentResultId, @FileAttachmentId)
ON CONFLICT (instrumentresultid, fileattachmentid) DO NOTHING";

        var inserted = 0;
        foreach (var fileAttachmentId in ids)
        {
            var rows = await connect.ExecuteAsync(insertSql, new { InstrumentResultId = requestModel.Id, FileAttachmentId = fileAttachmentId });
            inserted += rows;
        }

        logWriter?.LogInfo(
            $"ConfirmRequest instrumentResultId={requestModel.Id} status updated (statusid=883), attachmentIds requested={ids.Length} rowsInserted={inserted}",
            nameof(ConfirmRequestCommand),
            nameof(ExecuteAsync));

        return updated + inserted;
    }
}
