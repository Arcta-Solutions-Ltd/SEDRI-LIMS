using arc.app.Common;
using arc.app.Config;
using arc.app.Security;
using arc.common.Models.Coding;
using arc.data.Utils;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.identity;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class EditExpertRuleCommand : ICommandWithTypeReturningInteger<ExpertRule>
    {
        /// <summary>
        /// Updates an existing expert rule and its conditions, test conditions, actions, and specimen type filters.
        /// </summary>
        /// <param name="connect">Active database connection.</param>
        /// <param name="command">Expert rule with updated values; must include Id and SpecificationId.</param>
        /// <param name="logWriter">Logger for specimen type operations.</param>
        /// <returns>The ID of the updated expert rule.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExpertRule command, ILogWriter logWriter)
        {

            var queryFilter = new QueryFilterConfig().AddInteger("id", command.Id);
            var currentExpertRule = await new EditExpertRuleQuery().ExecuteAsync(connect, queryFilter);

            // When approval history exists, only CodingStatus Approved (145) allows Enabled to remain on; otherwise force Disabled.
            // When there is no approval row (null), preserve the submitted Enabled so operators can turn the rule on without an approval record.
            const int CodingStatusApproved = 145;
            var sqlCheck = @"SELECT CodingStatusId FROM expertruleapproval
                            WHERE ExpertRuleId = @Id
                            ORDER BY Id DESC LIMIT 1";
            var latestStatus = await connect.QueryFirstOrDefaultAsync<int?>(sqlCheck, new { command.Id });
            if (latestStatus.HasValue && latestStatus.Value != CodingStatusApproved)
            {
                command.Enabled = "No";
            }

            var resolver = new ResolveOrganismScope<ExpertRule>();
            command = await resolver.Resolve(connect, command, currentExpertRule);

            if (command.OrgGroupCodingId != currentExpertRule.OrgGroupCodingId)
            {
                logWriter.LogInfo(
                    $"Edit expert rule id={command.Id} organism group scope changed from {currentExpertRule.OrgGroupCodingId} to {command.OrgGroupCodingId}",
                    nameof(EditExpertRuleCommand),
                    nameof(ExecuteAsync));
            }

            if (command.GenusId > 0)
            {
                var organismFinder = new GetOrganismIdFromHierarchy(connect);
                command.OrganismId = command.OrganismId > 0 ? command.OrganismId : await organismFinder.Get(command.GenusId, command.SpeciesId, command.SubSpeciesId, command.SerotypeId, command.AdditionalId);
            }

            // update expert rule
            var sql = @"update ExpertRule set ExpertRuleName = @ExpertRuleName, RuleText = @RuleText, Enabled = @Enabled, AlertOnRule = @AlertOnRule, specificationid = @SpecificationId, TagId = @TagId,
                        orderid = @OrderId, familyId = @FamilyId, OrganismId = @OrganismId, OrgGroupCodingId = @OrgGroupCodingId, CombinationRule = @CombinationRule, lastmodifieddate = now() Where Id = @Id";
            await connect.ExecuteAsync(sql, command);

            // update rule conditions
            sql = @"delete from ExpertRuleCondition where ExpertRuleId = @RuleId";
            await connect.ExecuteAsync(sql, new { RuleId = command.Id });
            if (command.RuleConditionGrid != null)
            {
                foreach (var condition in command.RuleConditionGrid)
                {
                    sql = @"insert into ExpertRuleCondition(ExpertRuleId, AntibioticId, AntibioticGroupId, TestMethodId, SusceptibilityId, SpecialConsiderationId, StartVal, EndVal, LastModifiedDate)
                                            values(@ruleId, @AntibioticId, @AntibioticGroupId, @TestMethodId, @SusceptibilityId, @SpecialConsiderationId, @StartVal, @EndVal, now())";
                    await connect.ExecuteAsync(sql, new
                    {
                        RuleId = command.Id,
                        AntibioticId = condition.AntibioticId,
                        AntibioticGroupId = condition.AntibioticGroupId,
                        SusceptibilityId = condition.SusceptibilityId,
                        TestMethodId = condition.TestMethodId,
                        SpecialConsiderationId = condition.SpecialConsiderationId != 0 ? condition.SpecialConsiderationId : 973,
                        StartVal = condition.StartVal,
                        EndVal = condition.EndVal
                    });
                }
            }

            // update rule test conditions
            sql = @"delete from ExpertRuleTestCondition where ExpertRuleId = @RuleId";
            await connect.ExecuteAsync(sql, new { RuleId = command.Id });
            if (command.RuleTestConditionGrid != null)
            {
                foreach (var testcondition in command.RuleTestConditionGrid)
                {
                    sql = @"insert into ExpertRuleTestCondition(ExpertRuleId, TestName, FieldName, Comparison, CompValue, LastModifiedDate)
                                            values(@ruleId, @TestName, @FieldName, @Comparison, @CompValue, now())";
                    await connect.ExecuteAsync(sql, new
                    {
                        RuleId = command.Id,
                        TestName = testcondition.TestName,
                        FieldName = testcondition.FieldName,
                        Comparison = testcondition.Comparison,
                        CompValue = testcondition.CompValue,
                    });
                }
            }

            // update rule actions
            sql = @"delete from ExpertRuleAction where ExpertRuleId = @RuleId";
            await connect.ExecuteAsync(sql, new { RuleId = command.Id });
            if (command.RuleActionGrid != null)
            {
                foreach (var action in command.RuleActionGrid)
                {
                    if (ExpertRuleActionTargetNormalizer.NormalizeGridRow(action))
                    {
                        logWriter.LogInfo(
                            $"Edit expert rule id={command.Id} action grid row had both antibiotic and group targets; normalized to group only.",
                            nameof(EditExpertRuleCommand),
                            nameof(ExecuteAsync));
                    }

                    sql = @"insert into ExpertRuleAction(ExpertRuleId, AntibioticId, AntibioticGroupId, SusceptibilityId, DisplayOnReport, LastModifiedDate)
                                            values(@ruleId, @AntibioticId, @AntibioticGroupId, @SusceptibilityId, @DisplayOnReport, now())";
                    await connect.ExecuteAsync(sql, new
                    {
                        RuleId = command.Id,
                        AntibioticId = action.AntibioticId,
                        AntibioticGroupId = action.AntibioticGroupId,
                        SusceptibilityId = action.SusceptibilityId,
                        DisplayOnReport = action.DisplayOnReport
                    });
                }
            }

            // update specimen type dependencies

            // include
            sql = "select specimentypeid from expertrulespecimentype where expertruleid = @Id and included = true";
            var tempIncludeList = await connect.QueryAsync<int>(sql, command);
            var existingIncludeList = tempIncludeList.ToList();

            if (!string.IsNullOrEmpty(command.SpecimenTypesToInclude))
            {
                var typeIncludeList = command.SpecimenTypesToInclude.Split(",");
                foreach (var type in typeIncludeList)
                {
                    var typeAsNumber = int.Parse(type);
                    if (existingIncludeList.Contains(typeAsNumber))
                    {
                        existingIncludeList.Remove(typeAsNumber);
                    }
                    else
                    {
                        sql = @"insert into expertrulespecimentype(expertruleid, specimentypeid, included, lastmodifieddate)
                            values(@ExpertRuleId, @SpecimenTypeId, @Included, now())";
                        await connect.ExecuteAsync(sql, new { ExpertRuleId = command.Id, SpecimenTypeId = typeAsNumber, Included = true });
                    }
                }
            }
            foreach (var type in existingIncludeList)
            {
                sql = @"delete from expertrulespecimentype where expertruleid = @ExpertRuleId and specimentypeid = @SpecimenTypeId";
                await connect.ExecuteAsync(sql, new { ExpertRuleId = command.Id, SpecimenTypeId = type });
            }

            // exclude
            sql = "select specimentypeid from expertrulespecimentype where expertruleid = @Id and included = false";
            var tempExcludeList = await connect.QueryAsync<int>(sql, command);
            var existingExcludeList = tempExcludeList.ToList();
            
            if (!string.IsNullOrEmpty(command.SpecimenTypesToExclude))
            {
                var typeExcludeList = command.SpecimenTypesToExclude.Split(",");
                foreach (var type in typeExcludeList)
                {
                    var typeAsNumber = int.Parse(type);
                    if (existingExcludeList.Contains(typeAsNumber))
                    {
                        existingExcludeList.Remove(typeAsNumber);
                    }
                    else
                    {
                        sql = @"insert into expertrulespecimentype(expertruleid, specimentypeid, included, lastmodifieddate)
                            values(@ExpertRuleId, @SpecimenTypeId, @Included, now())";
                        await connect.ExecuteAsync(sql, new { ExpertRuleId = command.Id, SpecimenTypeId = typeAsNumber, Included = false });
                    }
                }
            }
            foreach (var type in existingExcludeList)
            {
                sql = @"delete from expertrulespecimentype where expertruleid = @ExpertRuleId and specimentypeid = @SpecimenTypeId";
                await connect.ExecuteAsync(sql, new { ExpertRuleId = command.Id, SpecimenTypeId = type });
            }

            return command.Id;
        }
    }
}
