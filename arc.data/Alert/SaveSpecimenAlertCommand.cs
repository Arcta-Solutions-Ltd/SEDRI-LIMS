using arc.app.Common;
using arc.domain.Alert;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Alert
{
    public class SaveSpecimenAlertCommand : ICommandWithTypeReturningInteger<SpecimenAlertCommandModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, SpecimenAlertCommandModel command, ILogWriter logWriter)
        {

            // Remove current alerts

            logWriter.LogInfo("Remoce current alerts", "SaveSpecimenAlertCommand", "Execute");
            string sql = @"delete from SpecimenAlert Where SpecimenId = @SpecimenId";
            await connect.ExecuteAsync(sql, new { SpecimenId = command.SpecimenId });

            // clear specimen record flags

            logWriter.LogInfo("Clear specimen record flags", "SaveSpecimenAlertCommand", "Execute");
            sql = @"update specimen set AlertTypeId = 0 Where Id = @SpecimenId";
            await connect.ExecuteAsync(sql, new { SpecimenId = command.SpecimenId });

            // clear the alert flags from tests

            logWriter.LogInfo("Clear the alert flags from tests", "SaveSpecimenAlertCommand", "Execute");
            sql = @"update tests set AlertTypeId = 0 Where SpecimenId = @SpecimenId";
            await connect.ExecuteAsync(sql, new { SpecimenId = command.SpecimenId });

            // clear the alert flags from cultures

            logWriter.LogInfo("Clear the alert flags from cultures", "SaveSpecimenAlertCommand", "Execute");
            sql = @"update culture set AlertTypeId = 0 Where SpecimenId = @SpecimenId";
            await connect.ExecuteAsync(sql, new { SpecimenId = command.SpecimenId });

            // clear the culture alert records

            logWriter.LogInfo("Clear the culture alert records", "SaveSpecimenAlertCommand", "Execute");
            sql = @"with cultures as (select Id from culture where SpecimenId = @SpecimenId)
                    delete from culturealert ca
                    using cultures c
                    where ca.cultureId = c.Id";
            await connect.ExecuteAsync(sql, new { SpecimenId = command.SpecimenId });

            // clear the culture records flag

            logWriter.LogInfo("Clear the culture records flag", "SaveSpecimenAlertCommand", "Execute");
            sql = @"update culture set AlertTypeId = @AlertTypeId Where SpecimenId = @SpecimenId";
            await connect.ExecuteAsync(sql, new { SpecimenId = command.SpecimenId });

            // clear the alert flags from culture tests

            logWriter.LogInfo("Clear the alert flags from culture tests", "SaveSpecimenAlertCommand", "Execute");
            sql = @"with cultures as (select Id from culture where SpecimenId = @SpecimenId)
                                update culturetests ct set AlertTypeId = 0 
                                from cultures c
                                Where c.Id = ct.cultureId";
            await connect.ExecuteAsync(sql, new { SpecimenId = command.SpecimenId });

            // clear the alert flags from ast tests

            logWriter.LogInfo("Clear the alert flags from ast tests", "SaveSpecimenAlertCommand", "Execute");
            sql = @"with cultures as (select Id from culture where SpecimenId = @SpecimenId)
                                update ast ct set AlertTypeId = 0 
                                from cultures c
                                Where c.Id = ct.cultureId";
            await connect.ExecuteAsync(sql, new { SpecimenId = command.SpecimenId });

            // Remove only alert-derived tags (preserve user-added tags from Add Tag form)
            var alertTagIds = command.Alerts
                .Where(a => !string.IsNullOrWhiteSpace(a.TagId))
                .SelectMany(a => a.TagId.Split(','))
                .Select(s => s.Trim())
                .Where(s => int.TryParse(s, out _))
                .Distinct()
                .ToList();
            if (alertTagIds.Count > 0)
            {
                logWriter.LogInfo("Remove alert-derived tags", "SaveSpecimenAlertCommand", "Execute");
                var tagIdArray = alertTagIds.Select(int.Parse).ToArray();
                sql = @"delete from SpecimenTag Where SpecimenId = @SpecimenId AND ListItemId = ANY(@TagIds)";
                await connect.ExecuteAsync(sql, new { SpecimenId = command.SpecimenId, TagIds = tagIdArray });
            }

            foreach (var alert in command.Alerts)
            {
                sql = @"insert into SpecimenAlert(SpecimenId, CultureId, AlertId, AlertTypeId, LastModifiedDate) values(@SpecimenId, @CultureId, @AlertId, @AlertTypeId, now()) returning id";
                var id = await connect.QueryFirstAsync(sql, alert);
                int alertId = (int)id.id;

                //Update the specimen record with the alert status

                logWriter.LogInfo("Update the specimen record with the alert status", "SaveSpecimenAlertCommand", "Execute");
                sql = @"update specimen set AlertTypeId = @AlertTypeId Where Id = @SpecimenId";
                await connect.ExecuteAsync(sql, new { SpecimenId = alert.SpecimenId, AlertTypeId = alert.AlertTypeId, TagId = alert.TagId });

                if (alert.SpecimenTestAlerts != null) 
                {
                    foreach (var testEntry in alert.SpecimenTestAlerts)
                    {
                        sql = @"update tests set AlertTypeId = @AlertTypeId Where Id = @TestId";
                        await connect.ExecuteAsync(sql, new { TestId = testEntry.TestId, AlertTypeId = alert.AlertTypeId });
                    }
                };

                //Add specimen tags
                
                logWriter.LogInfo("Add specimen tags", "SaveSpecimenAlertCommand", "Execute");
                if (! string.IsNullOrWhiteSpace(alert.TagId)) {
                    var tagList = alert.TagId.Split(",");
                    foreach (var tag in tagList)
                    {
                        sql = @"insert into SpecimenTag(SpecimenId, ListItemId, LastModifiedDate) values(@SpecimenId, @Tag, now()) returning id";
                        await connect.ExecuteAsync(sql, new { Tag = int.Parse(tag), SpecimenId = alert.SpecimenId });
                    }
                }

                if (alert.CultureAlerts != null)
                {
                    foreach (var cultureEntry in alert.CultureAlerts)
                    {
                        sql = @"insert into CultureAlert(CultureId, AlertId, AlertTypeId, LastModifiedDate) values(@CultureId, @AlertId, @AlertTypeId, now()) returning id";
                        id = await connect.QueryFirstAsync(sql, new { CultureId = cultureEntry.CultureId, AlertTypeId = alert.AlertTypeId, AlertId = alert.AlertId });
                        alertId = (int)id.id;

                        //Update the culture record with the alert status

                        logWriter.LogInfo("Update the culture record with the alert status", "SaveSpecimenAlertCommand", "Execute");
                        sql = @"update culture set AlertTypeId = @AlertTypeId Where Id = @CultureId";
                        await connect.ExecuteAsync(sql, new { CultureId = cultureEntry.CultureId, AlertTypeId = alert.AlertTypeId });

                        // update the alert flags from culture tests

                        logWriter.LogInfo("Update the alert flags from culture tests", "SaveSpecimenAlertCommand", "Execute");
                        if (cultureEntry.CultureTestAlerts != null)
                        {
                            foreach (var testEntry in cultureEntry.CultureTestAlerts)
                            {
                                sql = @"update culturetests set AlertTypeId = @AlertTypeId Where Id = @TestId";
                                await connect.ExecuteAsync(sql, new { TestId = testEntry.TestId, AlertTypeId = alert.AlertTypeId });
                            }
                        };

                        // update the ast alert flags from culture tests

                        logWriter.LogInfo("Update the ast alert flags from culture tests", "SaveSpecimenAlertCommand", "Execute");
                        if (cultureEntry.ASTTestAlerts != null)
                        {
                            foreach (var testEntry in cultureEntry.ASTTestAlerts)
                            {
                                sql = @"update ast set AlertTypeId = @AlertTypeId Where Id = @TestId";
                                await connect.ExecuteAsync(sql, new { TestId = testEntry.AstId, AlertTypeId = alert.AlertTypeId });
                            }
                        };
                    }
                }

            }

            return 0;
        }
    }
}
