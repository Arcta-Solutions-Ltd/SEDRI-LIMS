using arc.app.Common;
using arc.common.Models.SystemConfig;
using arc.domain.Configuration.ReportsConfig;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class AddSectionCommand : ICommandWithTypeReturningInteger<ReportSectionConfig>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ReportSectionConfig command, ILogWriter logWriter)
        {

            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };

            //insert report section
            var contents = JsonConvert.SerializeObject(command, settings);
            var sql = @"insert into Configs(ConfigName, ConfigTypeId, Contents, lastmodifieddate)
                    values(@ConfigName, @ConfigTypeId, cast(@Contents as json), now())";
            await connect.ExecuteAsync(sql, new { ConfigName = command.Name.ToLower(), ConfigTypeId = 14, Contents = contents });

            //Get the report

            var idList = command.Id.Split("|");
            var reportName = idList[0];
            sql = @"select * from Configs where ConfigName = @ConfigName";
            var reportContents = await connect.QueryFirstAsync<ConfigsModel>(sql, new { configName = reportName });
            var report = JsonConvert.DeserializeObject<ReportConfig>(reportContents.Contents);

            switch (idList[1].ToLower())
            {
                case "top sections":
                    report.AddMainSection(command.Name);
                    break;
                case "organism sections":
                    report.AddOrganismSection(command.Name);
                    break;
                case "bottom sections":
                    report.AddFinalSection(command.Name);
                    break;
            };

            sql = @"update Configs set Contents = to_json(@contents::jsonb), lastmodifieddate = now() Where  ConfigName = @ConfigName";
            await connect.ExecuteAsync(sql, new { Contents = JsonConvert.SerializeObject(report), ConfigName = reportName.ToLower() } );

            return 0;
        }
    }
}
