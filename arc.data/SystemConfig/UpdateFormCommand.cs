using arc.app.Common;
using arc.common.ExtensionMethods;
using arc.common.Models.SystemConfig;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.MappingsConfig;
using arc.domain.Configuration.PagesConfig;
using arc.domain.Configuration.QueryConfig;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig
{
    /// <summary>
    /// Writes a whole form configuration back to the Configs table, including its pages, save event,
    /// mappers, queries, data section and reports.
    /// </summary>
    internal class UpdateFormCommand : ICommandWithTypeReturningInteger<FullFormConfig>
    {
        /// <summary>
        /// ConfigType of a form row. Only used when the form has no row yet, which is the case for a
        /// form defined in code and edited for the first time; an existing row keeps the type it has.
        /// </summary>
        private const int FormConfigTypeId = 17;

        /// <summary>
        /// Persists every configuration row that makes up the form. Failures are logged rather than
        /// thrown, so callers should check the log when a saved change does not appear.
        /// </summary>
        /// <param name="connect">An open connection to the configuration database.</param>
        /// <param name="command">The full form configuration to write.</param>
        /// <param name="logWriter">Log writer used to record failures.</param>
        /// <returns>Zero.</returns>
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
                await UpdateConfigInDatabase(connect, form.Name, FormConfigTypeId, contents);

                //insert saveevent
                var eventDef = JsonConvert.DeserializeObject<EventConfig>(JsonConvert.SerializeObject(command.SaveEventConfig, settings));
                contents = JsonConvert.SerializeObject(eventDef, settings);
                await UpdateConfigInDatabase(connect, eventDef.EventName, 7, contents);

                //insert pages
                foreach (var page in command.PagesConfig)
                {
                    var newPage = JsonConvert.DeserializeObject<PageConfig>(JsonConvert.SerializeObject(page, settings));
                    contents = JsonConvert.SerializeObject(newPage, settings);
                    await UpdateConfigInDatabase(connect, newPage.Name, 9, contents);
                }

                //insert event mapping
                if (command.SaveEventConfig.MappingConfig != null)
                {
                    var eventMapping = JsonConvert.DeserializeObject<MapperConfig>(JsonConvert.SerializeObject(command.SaveEventConfig.MappingConfig, settings));
                    var target = command.SaveEventConfig.MappingConfig.GetTarget();
                    contents = JsonConvert.SerializeObject(eventMapping, settings);
                    contents = jsonReplacer.AddNewJsonValue(contents, "Target", target);
                    await UpdateConfigInDatabase(connect, eventMapping.Name, 12, contents);
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
                        await UpdateConfigInDatabase(connect, parameterMapping.Name, 10, contents);
                        command.InitialQueryConfig.ParameterMapperConfig = null;
                    }

                    // insert query result mapper
                    if (command.InitialQueryConfig.ResultMapperConfig != null)
                    {
                        var resultMapping = JsonConvert.DeserializeObject<MapperConfig>(JsonConvert.SerializeObject(command.InitialQueryConfig.ResultMapperConfig, settings));
                        var target = command.InitialQueryConfig.ResultMapperConfig.GetTarget();
                        contents = JsonConvert.SerializeObject(resultMapping, settings);
                        contents = jsonReplacer.AddNewJsonValue(contents, "Target", target);
                        await UpdateConfigInDatabase(connect, resultMapping.Name, 11, contents);
                        command.InitialQueryConfig.ResultMapperConfig = null;
                    }

                    contents = JsonConvert.SerializeObject(command.InitialQueryConfig, settings);
                    await UpdateConfigInDatabase(connect, command.InitialQueryConfig.Query, 8, contents);
                }

                // update datasection command
                if (command.DataSectionConfig != null)
                {
                    if (!string.IsNullOrWhiteSpace(command.DataSection)
                        && !command.DataSection.IsSameConfigName(command.DataSectionConfig.Name))
                    {
                        logWriter.LogWarning(
                            $"Form DataSection '{command.DataSection}' differs from DataSectionConfig.Name '{command.DataSectionConfig.Name}' for form '{command.Name}'",
                            nameof(UpdateFormCommand),
                            nameof(ExecuteAsync));
                    }

                    contents = JsonConvert.SerializeObject(command.DataSectionConfig, settings);
                    await UpdateConfigInDatabase(connect, command.DataSectionConfig.Name, 16, contents);
                }

                // report sections
                if (command.ReportSectionConfigList != null)
                {
                    foreach (var section in command.ReportSectionConfigList)
                    {
                        contents = JsonConvert.SerializeObject(section, settings);
                        await UpdateConfigInDatabase(connect, section.Name, 14, contents);
                    }
                }

                var sql = @"update Configs set Contents = to_json(@contents::jsonb), lastmodifieddate = now() Where  ConfigName = @ConfigName";

                //Amend reports
                foreach (var report in command.ReportConfigList)
                {
                    contents = JsonConvert.SerializeObject(report, settings);
                    await connect.ExecuteAsync(sql, new { ConfigName = report.Name.ToLower(), Contents = contents });
                }

                //Update the record view query mapper
                if (command.RecordViewQueryConfig != null)
                {
                    if (command.RecordViewQueryConfig.ResultMapperConfig != null)
                    {
                        var resultMapping = JsonConvert.DeserializeObject<MapperConfig>(JsonConvert.SerializeObject(command.RecordViewQueryConfig.ResultMapperConfig, settings));
                        var target = command.RecordViewQueryConfig.ResultMapperConfig.GetTarget();
                        contents = JsonConvert.SerializeObject(resultMapping, settings);
                        contents = jsonReplacer.AddNewJsonValue(contents, "Target", target);
                        await UpdateConfigInDatabase(connect, resultMapping.Name, 11, contents);
                        command.RecordViewQueryConfig.ResultMapperConfig = null;
                    }

                    contents = JsonConvert.SerializeObject(command.RecordViewQueryConfig, settings);
                    await UpdateConfigInDatabase(connect, command.RecordViewQueryConfig.Query, 8, contents);

                }
            }
            catch (Exception e)
            {
                logWriter.LogError(
                    $"Failed to save form configuration '{command?.Name}': {e.Message}. Configuration may be partially written. {e}",
                    nameof(UpdateFormCommand),
                    nameof(ExecuteAsync));
            }

            return 0;
        }

        /// <summary>
        /// Determines whether a configuration row already exists for the supplied name.
        /// </summary>
        /// <param name="connect">An open connection to the configuration database.</param>
        /// <param name="configName">The configuration name, already lower cased.</param>
        /// <returns>True when the row exists.</returns>
        private async Task<bool> DoesConfigExistInDatabase(NpgsqlConnection connect, string configName)
        {
            var sql = @"select c.Id, c.ConfigName from Configs c where c.ConfigName = @ConfigName";

            var resultList = await connect.QueryAsync<ConfigsModel>(sql, new { ConfigName = configName });

            return resultList.Count() != 0;
        }

        /// <summary>
        /// Inserts or updates a single configuration row. Names are stored lower cased; a blank name
        /// is ignored.
        /// </summary>
        /// <param name="connect">An open connection to the configuration database.</param>
        /// <param name="configName">The configuration name.</param>
        /// <param name="configTypeId">The configuration type identifier.</param>
        /// <param name="contents">The JSON contents to store.</param>
        private async Task UpdateConfigInDatabase(NpgsqlConnection connect, string configName, int configTypeId, string contents)
        {
            if (! string.IsNullOrWhiteSpace(configName))
            {
                configName = configName.ToLower();

                var updateSql = "update Configs set Contents = cast(@Contents as json), lastmodifieddate = now() where ConfigName = @configname";
                var insertSql = @"insert into Configs(ConfigName, ConfigTypeId, Contents, lastmodifieddate)
                                 values(@ConfigName, @ConfigTypeId, cast(@Contents as json), now())";

                var sqlToUse = await DoesConfigExistInDatabase(connect, configName) ? updateSql : insertSql;
                await connect.ExecuteAsync(sqlToUse, new { ConfigName = configName, ConfigTypeId = configTypeId, Contents = contents });
            }
        }
    }
}
