using arc.app.Common;
using arc.common.Models.Export;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    internal class AddExportProfileCommand : ICommandWithTypeReturningInteger<ExportProfileModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExportProfileModel command, ILogWriter logWriter = null)
        {
            // insert export profile record
            var sql = @"insert into exportprofile(name, description, modifieddate)
                        values(@name, @description, now()) returning id";
            var id = await connect.QueryFirstAsync(sql, command);
            var exportProfileId = (int)id.id;
            return exportProfileId;
        }
    }
}
