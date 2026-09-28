using arc.app.Common;
using arc.common.Models.Coding;
using arc.data.Utils;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class AddExpertRuleCommand : ICommandWithTypeReturningInteger<ExpertRule>
    {
        /// <summary>
        /// Inserts a new expert rule and its conditions, test conditions, actions, and specimen type filters.
        /// </summary>
        /// <param name="connect">Active database connection.</param>
        /// <param name="command">Expert rule to add; must include SpecificationId.</param>
        /// <param name="logWriter">Logger for specimen type inserts.</param>
        /// <returns>The ID of the newly created expert rule.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, ExpertRule command, ILogWriter logWriter)
        {

            // New expert rules always start with Enabled = 'No' until approved
            command.Enabled = "No";

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

            var sql = @"insert into ExpertRule(ExpertRuleName, RuleText, OrderId, FamilyId, OrganismId, OrgGroupCodingId, CombinationRule, Enabled, AlertOnRule, LastModifiedDate, SpecificationId, TagId)
                                   values(@ExpertRuleName, @RuleText, @OrderId, @FamilyId, @OrganismId, @OrgGroupCodingId, @CombinationRule, @Enabled, @AlertOnRule, now(), @SpecificationId, @TagId) returning id";
            var ruleId = await connect.QueryFirstAsync<int>(sql, command);

            if (command.RuleConditionGrid != null)
            {
                foreach (var condition in command.RuleConditionGrid)
                {
                    sql = @"insert into ExpertRuleCondition(ExpertRuleId, AntibioticId, AntibioticGroupId, TestMethodId, SusceptibilityId, SpecialConsiderationId, StartVal, EndVal, LastModifiedDate)
                                            values(@ruleId, @AntibioticId, @AntibioticGroupId, @TestMethodId, @SusceptibilityId, @SpecialConsiderationId, @StartVal, @EndVal, now())";
                    await connect.ExecuteAsync(sql, new
                    {
                        ruleId,
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

            if (command.RuleTestConditionGrid != null)
            {
                foreach (var testcondition in command.RuleTestConditionGrid)
                {
                    sql = @"insert into ExpertRuleTestCondition(ExpertRuleId, TestName, FieldName, Comparison, CompValue, LastModifiedDate)
                                            values(@ruleId, @TestName, @FieldName, @Comparison, @CompValue, now())";
                    await connect.ExecuteAsync(sql, new
                    {
                        ruleId,
                        TestName = testcondition.TestName,
                        FieldName = testcondition.FieldName,
                        Comparison = testcondition.Comparison,
                        CompValue = testcondition.CompValue
                    });
                }
            }

            if (command.RuleActionGrid != null)
            {
                foreach (var action in command.RuleActionGrid)
                {
                    if (ExpertRuleActionTargetNormalizer.NormalizeGridRow(action))
                    {
                        logWriter.LogInfo(
                            $"Add expert rule action grid row had both antibiotic and group targets; normalized to group only.",
                            nameof(AddExpertRuleCommand),
                            nameof(ExecuteAsync));
                    }

                    sql = @"insert into ExpertRuleAction(ExpertRuleId, AntibioticId, AntibioticGroupId, SusceptibilityId, DisplayOnReport, LastModifiedDate)
                                            values(@ruleId, @AntibioticId, @AntibioticGroupId, @SusceptibilityId, @DisplayOnReport, now())";
                    await connect.ExecuteAsync(sql, new
                    {
                        ruleId,
                        AntibioticId = action.AntibioticId,
                        AntibioticGroupId = action.AntibioticGroupId,
                        SusceptibilityId = action.SusceptibilityId,
                        DisplayOnReport = action.DisplayOnReport
                    });
                }
            }

            // add specimen type dependencies
            if (!string.IsNullOrWhiteSpace(command.SpecimenTypesToInclude))
            {
                var IncludeIds = command.SpecimenTypesToInclude.Split(",");
                logWriter.LogInfo($"Insert specimentypes for expert rule : {IncludeIds}", "AddExpertRuleCommand", "Execute");
                foreach (var type in IncludeIds)
                {
                    sql = @"insert into ExpertRuleSpecimenType(ExpertRuleId, SpecimenTypeId, Included, LastModifiedDate)
                                            values(@ruleId, @SpecimenTypeId, @Included, now())";
                    await connect.ExecuteAsync(sql, new
                    {
                        ruleId,
                        SpecimenTypeId = int.Parse(type),
                        Included = true
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(command.SpecimenTypesToExclude))
            {
                var ExcludeIds = command.SpecimenTypesToExclude.Split(",");
                logWriter.LogInfo($"Insert specimentypes for expert rule : {ExcludeIds}", "AddExpertRuleCommand", "Execute");
                foreach (var type in ExcludeIds)
                {
                    sql = @"insert into ExpertRuleSpecimenType(ExpertRuleId, SpecimenTypeId, Included, LastModifiedDate)
                                            values(@ruleId, @SpecimenTypeId, @Included, now())";
                    await connect.ExecuteAsync(sql, new
                    {
                        ruleId,
                        SpecimenTypeId = int.Parse(type),
                        Included = false
                    });
                }
            }

            return ruleId;
        }
    }
}
