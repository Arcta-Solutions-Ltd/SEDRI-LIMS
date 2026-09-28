using arc.app.Common;
using arc.common.Models.Admissions;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Admission;

/// <summary>
/// Command to replace all admission file attachments with the given list of file attachment IDs.
/// Deletes existing links for the admission, then inserts the new set.
/// </summary>
internal class SetAdmissionFileAttachmentsCommand : ICommandWithTypeReturningInteger<SetAdmissionFileAttachmentsModel>
{
    /// <summary>
    /// Executes the command: deletes existing admission file attachment links, then inserts new ones.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="command">The model containing admission ID and file attachment IDs.</param>
    /// <param name="logWriter">Optional log writer for debugging.</param>
    /// <returns>The last inserted id, or 0 if none inserted.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, SetAdmissionFileAttachmentsModel command, ILogWriter logWriter = null)
    {
        logWriter?.LogInfo($"SetAdmissionFileAttachments starting admissionId={command.AdmissionId}", "SetAdmissionFileAttachmentsCommand", "ExecuteAsync");

        var deleteSql = @"delete from admissionfileattachments where admissionid = @AdmissionId";
        await connect.ExecuteAsync(deleteSql, new { AdmissionId = command.AdmissionId });

        var ids = command.FileAttachmentIds?.Where(i => i > 0).Distinct().ToArray() ?? System.Array.Empty<int>();
        var lastId = 0;
        foreach (var fileAttachmentId in ids)
        {
            var insertSql = @"insert into admissionfileattachments(admissionid, fileattachmentid) values(@AdmissionId, @FileAttachmentId) returning id";
            var result = await connect.QueryFirstOrDefaultAsync<dynamic>(insertSql, new { AdmissionId = command.AdmissionId, FileAttachmentId = fileAttachmentId });
            if (result != null)
                lastId = (int)result.id;
        }

        logWriter?.LogInfo($"SetAdmissionFileAttachments completed admissionId={command.AdmissionId} count={ids.Length}", "SetAdmissionFileAttachmentsCommand", "ExecuteAsync");
        return lastId;
    }
}
