using arc.app.Common;
using arc.common.Models.Requests;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Request;

/// <summary>
/// Command to replace all request file attachments with the given list of file attachment IDs.
/// Deletes existing links for the request, then inserts the new set.
/// </summary>
internal class SetRequestFileAttachmentsCommand : ICommandWithTypeReturningInteger<SetRequestFileAttachmentsModel>
{
    /// <summary>
    /// Executes the command: deletes existing request file attachment links, then inserts new ones.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="command">The model containing request ID and file attachment IDs.</param>
    /// <param name="logWriter">Optional log writer for debugging.</param>
    /// <returns>The last inserted id, or 0 if none inserted.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, SetRequestFileAttachmentsModel command, ILogWriter logWriter = null)
    {
        logWriter?.LogInfo($"SetRequestFileAttachments starting requestId={command.RequestId}", "SetRequestFileAttachmentsCommand", "ExecuteAsync");

        var deleteSql = @"delete from requestfileattachments where requestid = @RequestId";
        await connect.ExecuteAsync(deleteSql, new { RequestId = command.RequestId });

        var ids = command.FileAttachmentIds?.Where(i => i > 0).Distinct().ToArray() ?? System.Array.Empty<int>();
        var lastId = 0;
        foreach (var fileAttachmentId in ids)
        {
            var insertSql = @"insert into requestfileattachments(requestid, fileattachmentid) values(@RequestId, @FileAttachmentId) returning id";
            var result = await connect.QueryFirstOrDefaultAsync<dynamic>(insertSql, new { RequestId = command.RequestId, FileAttachmentId = fileAttachmentId });
            if (result != null)
                lastId = (int)result.id;
        }

        logWriter?.LogInfo($"SetRequestFileAttachments completed requestId={command.RequestId} count={ids.Length}", "SetRequestFileAttachmentsCommand", "ExecuteAsync");
        return lastId;
    }
}
