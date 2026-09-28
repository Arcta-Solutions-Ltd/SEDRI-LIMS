using arc.app.Common;
using arc.common.Models.Alert;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Alert
{
    internal class AddAlertCommand : ICommandWithTypeReturningInteger<AlertDetailsModel>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, AlertDetailsModel command, ILogWriter logWriter)
        {
            command.Enabled = "No";
            command.DoesExist = string.IsNullOrWhiteSpace(command.DoesExist) ? "No" : "Yes";

            if (command.OrganismId > 0)
            {
                var hierachyUtil = new GetHierarchyFromOrganismId();
                var hierarchy = await hierachyUtil.ExecuteAsync(connect, new QueryFilterConfig().AddInteger("Id", command.OrganismId));
                command.OrderId = hierarchy.OrderId;
                command.FamilyId = hierarchy.FamilyId;
                command.GenusId = hierarchy.GenusId;
                command.SpeciesId = hierarchy.SpeciesId;
                command.AdditionalId = hierarchy.AdditionalId;
            }
            else
            {
                var organismFinder = new GetOrganismIdFromHierarchy(connect);
                command.OrganismId = command.OrganismId > 0 ? command.OrganismId : await organismFinder.Get(command.GenusId, command.SpeciesId, command.SubSpeciesId, command.SerotypeId, command.AdditionalId);
            };

            var sql = @"insert into Alert(AlertName, OrderId, FamilyId, OrganismId, OrgGroupCodingId, AlertMessage, TestAndOr, SusceptibilityAndOr, Enabled, LastModifiedDate, DoesExist, AlertTypeId, SpecificationId, TagId) 
                                   values(@AlertName, @OrderId, @FamilyId, @OrganismId, @OrgGroupCodingId, @AlertMessage, @TestAndOr, @SusceptibilityAndOr, @Enabled, now(), @DoesExist, @AlertTypeId, @SpecificationId, @TagId) returning id";
            var alertId = await connect.QueryFirstAsync<int>(sql, command);

            if (command.SusceptibilityGrid != null)
            {
                foreach (var susceptibility in command.SusceptibilityGrid)
                {
                    sql = @"insert into AlertLines(AlertId, AntibioticId, SusceptibilityId, LastModifiedDate) 
                                            values(@AlertId, @AntibioticId, @SusceptibilityId, now())";
                    await connect.ExecuteAsync(sql, new { alertId, susceptibility.AntibioticId, susceptibility.SusceptibilityId });
                }
            }

            if (command.TestGrid != null)
            {
                foreach (var test in command.TestGrid)
                {
                    var compValue = "";
                    if (!string.IsNullOrWhiteSpace(test.ListValue))
                    {
                        compValue = test.ListValue;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(test.NumberValue))
                        {
                            compValue = test.NumberValue;
                        }
                        else
                        {
                            compValue = test.StringValue;
                        }
                    }

                    sql = @"insert into AlertTestLines(AlertId, TestName, FieldName, Comparison, CompValue, LastModifiedDate) 
                                                values(@AlertId, @TestName, @FieldName, @Comparison, @CompValue, now())";
                    await connect.ExecuteAsync(sql, new { alertId, TestName = test.Test, FieldName = test.Field, test.Comparison, compValue });
                }
            }

            return alertId;
        }
    }
}
