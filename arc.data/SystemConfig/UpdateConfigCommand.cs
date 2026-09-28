using arc.app.Common;
using arc.data.model.Configuration;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class UpdateConfigCommand : ICommandWithTypeReturningInteger<ConfigsDataModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ConfigsDataModel command, ILogWriter logWriter)
        {
            var sql = @"select c.Id, c.ConfigName, c.ConfigTypeId, c.contents from Configs c
                        where c.ConfigName = @ConfigName
                        order by ConfigName";

            var resultList = await connect.QueryAsync<ConfigsDataModel>(sql, new { ConfigName = command.ConfigName.ToLower() });

            if (resultList.Count() == 0)
            {
                command.ConfigName = command.ConfigName.ToLower();
                sql = @"insert into Configs(ConfigName, ConfigTypeId, Contents, lastmodifieddate)
                        values(@ConfigName, @ConfigTypeId, to_json(@contents::jsonb), now()) returning id";
                var id = await connect.QueryFirstAsync(sql, command);
                command.Id = (int)id.id;
            } else
            {
                sql = @"update Configs set Contents = to_json(@contents::jsonb), lastmodifieddate = now() Where  ConfigName = @ConfigName";
                await connect.ExecuteAsync(sql, new { ConfigName = command.ConfigName.ToLower(), Contents = command.Contents });
            }

            return command.Id;
        }
    }
}