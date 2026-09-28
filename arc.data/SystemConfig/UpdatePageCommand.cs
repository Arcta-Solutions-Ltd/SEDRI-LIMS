using arc.app.Common;
using arc.common.Models.SystemConfig;
using arc.domain.Configuration.PagesConfig;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class UpdatePageCommand : ICommandWithTypeReturningInteger<PageConfig>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, PageConfig command, ILogWriter logWriter)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };

            //Check whether page exists
            var sql = @"select * from Configs where ConfigName = @ConfigName and ConfigTypeId = 9";
            var pageList = await connect.QueryAsync<ConfigsModel>(sql, new { ConfigName = command.Name });
            var contents = JsonConvert.SerializeObject(command, settings);

            //If exists then update the page definition

            if (pageList.Count() > 0)
            {
                sql = @"update Configs set Contents =cast(@Contents as json), lastmodifieddate = now() where ConfigName = @ConfigName and ConfigTypeId = 9";
                await connect.ExecuteAsync(sql, new { ConfigName = command.Name, Contents = contents });
            }

            //If does not exist then create the page definition

            if (pageList.Count() == 0)
            {
                sql = @"insert into Configs(ConfigName, ConfigTypeId, Contents, lastmodifieddate)
                        values(@ConfigName, @ConfigTypeId, cast(@Contents as json), now())";
                await connect.ExecuteAsync(sql, new { ConfigName = command.Name, ConfigTypeId = 9, Contents = contents });
            }

            return 0;
        }
    }
}