using arc.app.Common;
using arc.common.Models.Export;
using Dapper;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    internal class AddExportProfileFieldCommand : ICommandWithTypeReturningInteger<ExportProfileFieldModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExportProfileFieldModel command, ILogWriter logWriter = null)
        {
            // insert export profile record
            command.ModifiedDate = DateTime.UtcNow;
            var sql = @"insert into exportprofilerecord(exportprofileid, tablename, fieldname, headername, formname, labelname, modifieddate,ordernumber, moredata)
                        values(@exportprofileid, @tablename,@fieldname,@headername,@formname,@labelname,@modifieddate,@ordernumber,cast(@MoreData as json)) returning id";
            var id = await connect.QueryFirstAsync(sql, command);
            var exportProfileFieldId = (int)id.id;
            return exportProfileFieldId;
        }
    }
}
