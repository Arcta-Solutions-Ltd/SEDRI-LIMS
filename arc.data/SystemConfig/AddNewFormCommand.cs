using arc.app.Common;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.MappingsConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.UIEventsConfig;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    internal class AddNewFormCommand : ICommandWithTypeReturningInteger<FullFormConfig>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, FullFormConfig command, ILogWriter logWriter)
        {
            try
            {
                var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
                var jsonElementRemover = new JsonElementRemover();
                var jsonReplacer = new JsonReplacer(jsonElementRemover);

                //insert form
                var form = JsonConvert.DeserializeObject<FormConfig>(JsonConvert.SerializeObject(command, settings));
                var contents = JsonConvert.SerializeObject(form, settings);
                var sql = @"insert into Configs(ConfigName, ConfigTypeId, Contents, lastmodifieddate)
                        values(@ConfigName, @ConfigTypeId, cast(@Contents as json), now())";
                await connect.ExecuteAsync(sql, new { ConfigName = form.Name, ConfigTypeId = command.Type, Contents = contents });

                //insert into uievent
                var uievent = JsonConvert.DeserializeObject<UIEventConfig>(JsonConvert.SerializeObject(command.UIEventConfig, settings));
                contents = JsonConvert.SerializeObject(uievent, settings);
                await connect.ExecuteAsync(sql, new { ConfigName = uievent.Name, ConfigTypeId = 6, Contents = contents });

                //insert saveevent
                var eventDef = JsonConvert.DeserializeObject<EventConfig>(JsonConvert.SerializeObject(command.SaveEventConfig, settings));
                contents = JsonConvert.SerializeObject(eventDef, settings);
                await connect.ExecuteAsync(sql, new { ConfigName = eventDef.EventName, ConfigTypeId = 7, Contents = contents });

                //insert pages
                foreach (var page in command.PagesConfig)
                {
                    var newPage = JsonConvert.DeserializeObject<PageConfig>(JsonConvert.SerializeObject(page, settings));
                    contents = JsonConvert.SerializeObject(newPage, settings);
                    await connect.ExecuteAsync(sql, new { ConfigName = newPage.Name, ConfigTypeId = 9, Contents = contents });
                }

                //insert event mapping
                if (command.SaveEventConfig.MappingConfig != null)
                {
                    var eventMapping = JsonConvert.DeserializeObject<MapperConfig>(JsonConvert.SerializeObject(command.SaveEventConfig.MappingConfig, settings));
                    var target = command.SaveEventConfig.MappingConfig.GetTarget();
                    contents = JsonConvert.SerializeObject(eventMapping, settings);
                    contents = jsonReplacer.AddNewJsonValue(contents, "Target", target);
                    await connect.ExecuteAsync(sql, new { ConfigName = eventMapping.Name, ConfigTypeId = 12, Contents = contents });
                }

                //insert initial query
                if (command.InitialQueryConfig != null)
                {
                    //insert query parameter mapper
                    if (command.InitialQueryConfig.ParameterMapperConfig != null)
                    {
                        var parameterMapping = JsonConvert.DeserializeObject<MapperConfig>(JsonConvert.SerializeObject(command.InitialQueryConfig.ParameterMapperConfig, settings));
                        var target = command.InitialQueryConfig.ParameterMapperConfig.GetTarget();
                        contents = JsonConvert.SerializeObject(parameterMapping, settings);
                        contents = jsonReplacer.AddNewJsonValue(contents, "Target", target);
                        await connect.ExecuteAsync(sql, new { ConfigName = parameterMapping.Name, ConfigTypeId = 10, Contents = contents });
                        command.InitialQueryConfig.ParameterMapperConfig = null;
                    }

                    // insert query result mapper
                    if (command.InitialQueryConfig.ResultMapperConfig != null)
                    {
                        var resultMapping = JsonConvert.DeserializeObject<MapperConfig>(JsonConvert.SerializeObject(command.InitialQueryConfig.ResultMapperConfig, settings));
                        var target = command.InitialQueryConfig.ResultMapperConfig.GetTarget();
                        contents = JsonConvert.SerializeObject(resultMapping, settings);
                        contents = jsonReplacer.AddNewJsonValue(contents, "Target", target);
                        await connect.ExecuteAsync(sql, new { ConfigName = resultMapping.Name, ConfigTypeId = 11, Contents = contents });
                        command.InitialQueryConfig.ResultMapperConfig = null;
                    }

                    contents = JsonConvert.SerializeObject(command.InitialQueryConfig, settings);
                    await connect.ExecuteAsync(sql, new { ConfigName = command.InitialQueryConfig.Query, ConfigTypeId = 8, Contents = contents });
                }

                // add new datasection command
                if (command.DataSectionConfig != null)
                {
                    contents = JsonConvert.SerializeObject(command.DataSectionConfig, settings);
                    await connect.ExecuteAsync(sql, new { ConfigName = command.DataSectionConfig.Name, ConfigTypeId = 16, Contents = contents });
                }

                // add report section command
                if (command.NewSectionConfig != null)
                {
                    contents = JsonConvert.SerializeObject(command.NewSectionConfig, settings);
                    await connect.ExecuteAsync(sql, new { ConfigName = command.NewSectionConfig.Name, ConfigTypeId = 14, Contents = contents });
                }

                sql = @"update Configs set Contents = to_json(@contents::jsonb), lastmodifieddate = now() Where  ConfigName = @ConfigName";

                //Amend reports
                foreach (var report in command.ReportConfigList)
                {
                    contents = JsonConvert.SerializeObject(report, settings);
                    await connect.ExecuteAsync(sql, new { ConfigName = report.Name.ToLower(), Contents = contents });
                }
            }
            catch (Exception e)
            {
                logWriter.LogError(e.Message, "AddNewFormCommand", "Execute");
                throw;
            }


            return 0;
        }
    }
}
