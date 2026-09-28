using arc.app.Common;
using arc.domain.Configuration.FormStructureConfig;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class DeleteFormCommand : ICommandWithTypeReturningInteger<FullFormConfig>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, FullFormConfig command, ILogWriter logWriter)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };

            var sql = @"delete from configs where configname = @ConfigName";

            //delete form
            await connect.ExecuteAsync(sql, new { ConfigName = command.Name });

            //delete uievent
            await connect.ExecuteAsync(sql, new { ConfigName = command.UIEventConfig.Name });

            //delete saveevent
            await connect.ExecuteAsync(sql, new { ConfigName = command.SaveEventConfig.EventName });

            //delete pages
            foreach (var page in command.PagesConfig)
            {
                await connect.ExecuteAsync(sql, new { ConfigName = page.Name });
            }

            //delete event mapping
            if (command.SaveEventConfig.MappingConfig != null)
            {
                await connect.ExecuteAsync(sql, new { ConfigName = command.SaveEventConfig.MappingConfig.Name });
            }

            //delete initial query
            if (command.InitialQueryConfig != null)
            {
                await connect.ExecuteAsync(sql, new { ConfigName = command.InitialQueryConfig.Query });

                //delete query parameter mapper
                if (command.InitialQueryConfig.ParameterMapperConfig != null)
                {
                    await connect.ExecuteAsync(sql, new { ConfigName = command.InitialQueryConfig.ParameterMapperConfig.Name });
                }

                //delete query result mapper
                if (command.InitialQueryConfig.ResultMapperConfig != null)
                {
                    await connect.ExecuteAsync(sql, new { ConfigName = command.InitialQueryConfig.ResultMapperConfig.Name });
                }
            }

            //delete the data section
            if (command.DataSectionConfig != null)
            {
                await connect.ExecuteAsync(sql, new { ConfigName = command.DataSectionConfig.Name });
            }

            //delete report sections
            foreach (var section in command.ReportSectionConfigList)
            {
                await connect.ExecuteAsync(sql, new { ConfigName = section.Name });
            }

            //Amend reports
            foreach (var report in command.ReportConfigList)
            {
                var contents = JsonConvert.SerializeObject(report, settings);
                sql = @"update Configs set Contents = to_json(@contents::jsonb), lastmodifieddate = now() Where  ConfigName = @ConfigName";
                await connect.ExecuteAsync(sql, new { ConfigName = report.Name.ToLower(), Contents = contents });
            }


            return 0;
        }
    }
}
