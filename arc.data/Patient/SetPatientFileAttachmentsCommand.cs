using arc.app.Common;
using arc.common.Models.Patient;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Patient
{
    /// <summary>
    /// Command to replace all patient file attachments with the given list of file attachment IDs.
    /// Deletes existing links for the patient, then inserts the new set.
    /// </summary>
    internal class SetPatientFileAttachmentsCommand : ICommandWithTypeReturningInteger<SetPatientFileAttachmentsModel>
    {
        /// <summary>
        /// Executes the command: deletes existing patient file attachment links, then inserts new ones.
        /// </summary>
        /// <param name="connect">The database connection.</param>
        /// <param name="command">The model containing patient ID and file attachment IDs.</param>
        /// <param name="logWriter">Optional log writer for debugging.</param>
        /// <returns>The last inserted id, or 0 if none inserted.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, SetPatientFileAttachmentsModel command, ILogWriter logWriter = null)
        {
            logWriter?.LogInfo($"SetPatientFileAttachments starting patientId={command.PatientId}", "SetPatientFileAttachmentsCommand", "ExecuteAsync");

            var deleteSql = @"delete from patientfileattachments where patientid = @PatientId";
            await connect.ExecuteAsync(deleteSql, new { PatientId = command.PatientId });

            var ids = command.FileAttachmentIds?.Where(i => i > 0).Distinct().ToArray() ?? System.Array.Empty<int>();
            var lastId = 0;
            foreach (var fileAttachmentId in ids)
            {
                var insertSql = @"insert into patientfileattachments(patientid, fileattachmentid) values(@PatientId, @FileAttachmentId) returning id";
                var result = await connect.QueryFirstOrDefaultAsync<dynamic>(insertSql, new { PatientId = command.PatientId, FileAttachmentId = fileAttachmentId });
                if (result != null)
                    lastId = (int)result.id;
            }

            logWriter?.LogInfo($"SetPatientFileAttachments completed patientId={command.PatientId} count={ids.Length}", "SetPatientFileAttachmentsCommand", "ExecuteAsync");
            return lastId;
        }
    }
}
