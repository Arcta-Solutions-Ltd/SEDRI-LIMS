using arc.app.Common;
using arc.common.Utils;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.ReportsConfig;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class AddNewReportCommand : ICommandWithTypeReturningInteger<FullReportConfig>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, FullReportConfig command, ILogWriter logWriter)
        {
            try
            {
                var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
                var jsonElementRemover = new JsonElementRemover();
                var jsonReplacer = new JsonReplacer(jsonElementRemover);

                //insert report
                var form = JsonConvert.DeserializeObject<ReportConfig>(JsonConvert.SerializeObject(command, settings));
                var contents = JsonConvert.SerializeObject(form, settings);
                var sql = @"insert into Configs(ConfigName, ConfigTypeId, Contents, lastmodifieddate)
                        values(@ConfigName, @ConfigTypeId, cast(@Contents as json), now())";
                await connect.ExecuteAsync(sql, new { ConfigName = form.Name, ConfigTypeId = 13, Contents = contents });

                //Insert main sections
                foreach (var section in command.MainSectionsConfig)
                {
                    //var newSection = JsonConvert.DeserializeObject<ReportSectionConfig>(JsonConvert.SerializeObject(section, settings));
                    contents = JsonConvert.SerializeObject(section, settings);
                    await connect.ExecuteAsync(sql, new { ConfigName = section.Name, ConfigTypeId = 14, Contents = contents });
                }

                //Insert final sections
                foreach (var section in command.FinalSectionsConfig)
                {
                    //var newSection = JsonConvert.DeserializeObject<ReportSectionConfig>(JsonConvert.SerializeObject(section, settings));
                    contents = JsonConvert.SerializeObject(section, settings);
                    await connect.ExecuteAsync(sql, new { ConfigName = section.Name, ConfigTypeId = 14, Contents = contents });
                }

                //Insert organism sections
                foreach (var section in command.OrganismSectionsConfig)
                {
                    //var newSection = JsonConvert.DeserializeObject<ReportSectionConfig>(JsonConvert.SerializeObject(section, settings));
                    contents = JsonConvert.SerializeObject(section, settings);
                    await connect.ExecuteAsync(sql, new { ConfigName = section.Name, ConfigTypeId = 15, Contents = contents });
                }
            }
            catch (Exception e)
            {
                logWriter.LogError(e.Message, "AddNewReportCommand", "Execute");
            }


            return 0;
        }
    }
}
