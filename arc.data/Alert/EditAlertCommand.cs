using arc.app.Common;
using arc.data.Utils;
using arc.domain.Alert;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Alert
{
    internal class EditAlertCommand : ICommandWithTypeReturningInteger<AlertDetails>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, AlertDetails command, ILogWriter logWriter)
        {
            var queryFilter = new QueryFilterConfig().AddInteger("id", command.Id);
            var currentAlert = await new EditAlertQuery().ExecuteAsync(connect, queryFilter);

            var resolver = new ResolveOrganismScope<AlertDetails>();
            command = await resolver.Resolve(connect, command, currentAlert);

            if (command.GenusId > 0)
            {
                var organismFinder = new GetOrganismIdFromHierarchy(connect);

                command.OrganismId = command.OrganismId > 0 ? command.OrganismId : await organismFinder.Get(command.GenusId, command.SpeciesId, command.SubSpeciesId, command.SerotypeId, command.AdditionalId);
            }

            // Force Enabled to 'No' if alert is not approved
            const int CodingStatusApproved = 145;
            var sqlCheck = @"SELECT CodingStatusId FROM alertapproval
                            WHERE AlertId = @Id
                            ORDER BY Id DESC LIMIT 1";
            var latestStatus = await connect.QueryFirstOrDefaultAsync<int?>(sqlCheck, new { command.Id });
            if (latestStatus != CodingStatusApproved)
            {
                command.Enabled = "No";
            }

            // update alert

            var sql = @"update Alert set alertName = @AlertName, alertMessage = @AlertMessage, testAndOr = @TestAndOr, susceptibilityAndOr = @SusceptibilityAndOr,
                        enabled = @Enabled, doesExist = @DoesExist, alertTypeId = @AlertTypeId, specificationId = @SpecificationId, tagId = @TagId,
                        orderid = @OrderId, familyId = @FamilyId, OrganismId = @OrganismId, OrgGroupCodingId = @OrgGroupCodingId, lastmodifieddate = now() Where  Id = @Id";
            await connect.ExecuteAsync(sql, command);

            // update alert lines

            sql = @"delete from AlertLines where AlertId = @AlertId";
            await connect.ExecuteAsync(sql, new { AlertId = command.Id });

            if (command.SusceptibilityGrid != null)
            {
                foreach (var susceptibility in command.SusceptibilityGrid)
                {
                    sql = @"insert into AlertLines(AlertId, AntibioticId, SusceptibilityId, LastModifiedDate) 
                                            values(@AlertId, @AntibioticId, @SusceptibilityId, now())";
                    await connect.ExecuteAsync(sql, new { alertId = command.Id, antibioticId = susceptibility.AntibioticId, susceptibilityId = susceptibility.SusceptibilityId });
                }
            }

            // update alert test lines

            sql = @"delete from AlertTestLines where AlertId = @AlertId";
            await connect.ExecuteAsync(sql, new { AlertId = command.Id });

            if (command.TestGrid != null)
            {
                foreach (var test in command.TestGrid)
                {
                    sql = @"insert into AlertTestLines(AlertId, TestName, FieldName, Comparison, CompValue, LastModifiedDate) 
                        values(@AlertId, @TestName, @FieldName, @Comparison, @CompValue, now())";
                    await connect.ExecuteAsync(sql, new { AlertId = command.Id, test.TestName, test.FieldName, test.Comparison, test.CompValue });
                }
            }

            return command.Id;
        }
    }
}
