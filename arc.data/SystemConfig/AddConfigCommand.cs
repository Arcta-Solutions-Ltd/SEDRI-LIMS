using arc.app.Common;
using arc.common.Models.SystemConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class AddConfigCommand : ICommandWithTypeReturningInteger<ConfigsModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ConfigsModel command, ILogWriter logWriter)
        {
            var sql = @"insert into Configs(ConfigName, ConfigTypeId, Contents, lastmodifieddate)
                        values(@ConfigName, @ConfigTypeId, @Contents, now()) returning id";
            var id = await connect.QueryFirstAsync(sql, command);
            var configId = (int)id.id;

            return configId;
        }
    }
}
