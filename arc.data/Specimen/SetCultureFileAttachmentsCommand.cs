using arc.app.Common;
using arc.common.Models.Specimen;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen
{
    /// <summary>
    /// Command to replace all culture file attachments with the given list of file attachment IDs.
    /// Deletes existing links for the culture, then inserts the new set.
    /// </summary>
    internal class SetCultureFileAttachmentsCommand : ICommandWithTypeReturningInteger<SetCultureFileAttachmentsModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, SetCultureFileAttachmentsModel command, ILogWriter logWriter = null)
        {
            logWriter?.LogInfo($"SetCultureFileAttachments starting cultureId={command.CultureId}", "SetCultureFileAttachmentsCommand", "ExecuteAsync");

            var deleteSql = @"delete from culturefileattachments where cultureid = @CultureId";
            await connect.ExecuteAsync(deleteSql, new { CultureId = command.CultureId });

            var ids = command.FileAttachmentIds?.Where(i => i > 0).Distinct().ToArray() ?? System.Array.Empty<int>();
            var lastId = 0;
            foreach (var fileAttachmentId in ids)
            {
                var insertSql = @"insert into culturefileattachments(cultureid, fileattachmentid) values(@CultureId, @FileAttachmentId) returning id";
                var result = await connect.QueryFirstOrDefaultAsync<dynamic>(insertSql, new { CultureId = command.CultureId, FileAttachmentId = fileAttachmentId });
                if (result != null)
                    lastId = (int)result.id;
            }

            logWriter?.LogInfo($"SetCultureFileAttachments completed cultureId={command.CultureId} count={ids.Length}", "SetCultureFileAttachmentsCommand", "ExecuteAsync");
            return lastId;
        }
    }
}
