using arc.app.Common;
using arc.data.model.Configuration;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class EditConfigCommand : ICommandWithTypeReturningInteger<ConfigsDataModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ConfigsDataModel command, ILogWriter logWriter)
        {
            var sql = @"update Configs set Contents = to_json(@contents::jsonb), lastmodifieddate = now() Where  ConfigName = @ConfigName";

            await connect.ExecuteAsync(sql, command);

            return command.Id;
        }
    }
}

