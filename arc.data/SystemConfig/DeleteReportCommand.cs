using arc.app.Common;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.ReportsConfig;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class DeleteReportCommand : ICommandWithTypeReturningInteger<FullReportConfig>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, FullReportConfig command, ILogWriter logWriter)
        {
            try
            {
                var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };

                //delete report
                var report = JsonConvert.DeserializeObject<ReportConfig>(JsonConvert.SerializeObject(command, settings));
                var sql = @"delete from Configs Where ConfigName = @ConfigName";
                await connect.ExecuteAsync(sql, new { ConfigName = report.Name });

                //Delete sections
                foreach (var section in command.MainSectionsConfig)
                {
                    await connect.ExecuteAsync(sql, new { ConfigName = section.Name });
                }
            }
            catch (Exception e)
            {
                logWriter.LogError(e.Message, "DeleteReportCommand", "Execute");
            }

            return 0;
        }
    }
}
