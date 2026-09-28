using arc.app.Common;
using arc.common.Models.Export;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    internal class AddExportRunHistoryCommand : ICommandWithTypeReturningInteger<ExportRunRequestModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExportRunRequestModel command, ILogWriter logWriter = null)
        {
            var filterJsonString = JsonConvert.SerializeObject(command).Replace("\r\n", "");

            var sql = @"insert into exportrunhistory(exportprofileid, filter, runat, fileattachmentid, exportscheduleid) 
                          values(@exportprofileid, to_json(@filterjsonstring::jsonb), now(), @fileattachmentid, @exportscheduleid) returning id";

            var id = await connect.QueryFirstAsync(sql, new
            {
                exportprofileid = int.Parse(command.ExportProfileId),
                filterJsonString = filterJsonString,
                fileattachmentid = command.FileAttachmentId,
                exportscheduleid = command.ExportScheduleId
            });

            var historyId = (int)id.id;

            return historyId;
        }
    }
}
