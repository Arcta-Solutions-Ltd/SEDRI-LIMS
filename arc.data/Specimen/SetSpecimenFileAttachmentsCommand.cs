using arc.app.Common;
using arc.common.Models.Specimen;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen
{
    /// <summary>
    /// Command to replace all specimen file attachments with the given list of file attachment IDs.
    /// Deletes existing links for the specimen, then inserts the new set.
    /// </summary>
    internal class SetSpecimenFileAttachmentsCommand : ICommandWithTypeReturningInteger<SetSpecimenFileAttachmentsModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, SetSpecimenFileAttachmentsModel command, ILogWriter logWriter = null)
        {
            logWriter?.LogInfo($"SetSpecimenFileAttachments starting specimenId={command.SpecimenId}", "SetSpecimenFileAttachmentsCommand", "ExecuteAsync");

            var deleteSql = @"delete from specimenfileattachments where specimenid = @SpecimenId";
            await connect.ExecuteAsync(deleteSql, new { SpecimenId = command.SpecimenId });

            var ids = command.FileAttachmentIds?.Where(i => i > 0).Distinct().ToArray() ?? System.Array.Empty<int>();
            var lastId = 0;
            foreach (var fileAttachmentId in ids)
            {
                var insertSql = @"insert into specimenfileattachments(specimenid, fileattachmentid) values(@SpecimenId, @FileAttachmentId) returning id";
                var result = await connect.QueryFirstOrDefaultAsync<dynamic>(insertSql, new { SpecimenId = command.SpecimenId, FileAttachmentId = fileAttachmentId });
                if (result != null)
                    lastId = (int)result.id;
            }

            logWriter?.LogInfo($"SetSpecimenFileAttachments completed specimenId={command.SpecimenId} count={ids.Length}", "SetSpecimenFileAttachmentsCommand", "ExecuteAsync");
            return lastId;
        }
    }
}
