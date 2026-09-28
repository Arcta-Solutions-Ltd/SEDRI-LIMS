using arc.app.Common;
using arc.common.Models.Export;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    internal class EditExportProfileCommand : ICommandWithTypeReturningInteger<ExportProfileModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExportProfileModel command, ILogWriter logWriter = null)
        {
            var sql = @"update exportprofile set name = @Name, description = @Description, modifieddate = now() where id = @Id";
            await connect.ExecuteAsync(sql, command);
            return command.Id;
        }
    }
}
