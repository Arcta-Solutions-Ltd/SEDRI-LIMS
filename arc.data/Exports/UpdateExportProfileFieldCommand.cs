using arc.app.Common;
using arc.common.Models.Export;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    public class UpdateExportProfileFieldCommand : ICommandWithTypeReturningInteger<ExportProfileFieldModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExportProfileFieldModel command, ILogWriter logWriter = null)
        {
                // update export profile field 
                command.ModifiedDate = DateTime.UtcNow;
                var sql = @"update exportprofilerecord set tablename = @TableName, fieldname = @FieldName, headername =@HeaderName,
                        formname=@FormName, labelname =@LabelName, modifieddate=@ModifiedDate, ordernumber = @OrderNumber where id=@Id returning id";
                var id = await connect.QueryFirstAsync(sql, command);
                var exportProfileFieldId = (int)id.id;
                return exportProfileFieldId;
        }
    }
}
