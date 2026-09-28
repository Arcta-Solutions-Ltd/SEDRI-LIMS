using arc.common.Models.Coding;
using arc.common.Models.AST;
using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace arc.data.Coding
{
    internal class ExpertRuleActionsQuery : IQueryReturningType<List<ExpertRuleActionReturnModel>>
    {
        /// <summary>
        /// Retrieves expert rule actions applicable to an AST row. Filters by organism, specification (via guidelinesid), and specimen type.
        /// </summary>
        /// <param name="connect">Active database connection.</param>
        /// <param name="queryFilters">Must contain: organismid, guidelines, testresult, antibiotic, testmethod, specialconsiderationid, specimentypeid.</param>
        /// <returns>List of expert rule actions for the given context.</returns>
        public async Task<List<ExpertRuleActionReturnModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var organismid = queryFilters.GetIntegerValue("organismid");
            var guidelinesId = queryFilters.GetIntegerValue("guidelines");
            var susceptibilityid = queryFilters.GetIntegerValue("testresult");
            var antibioticid = queryFilters.GetIntegerValue("antibiotic");
            var testmethodid = queryFilters.GetIntegerValue("testmethod");
            var specialconsiderationid = queryFilters.GetIntegerValue("specialconsiderationid");
            var specimenTypeId = queryFilters.GetIntegerValue("specimentypeid");

            var sql = @"select r.* from expertrule r
                inner join specification s on r.specificationid = s.id and s.guidelinesid = @GuidelinesId
                where r.organismid = @OrganismId and r.enabled = 'Yes'
                and (not exists (select 1 from expertrulespecimentype ert where ert.expertruleid = r.id) or exists (select 1 from expertrulespecimentype ert where ert.expertruleid = r.id and ert.specimentypeid = @SpecimenTypeId and ert.included = true))";
            var rules = await connect.QueryAsync<ExpertRuleDetailsModel>(sql, new { OrganismId = organismid, GuidelinesId = guidelinesId, SpecimenTypeId = specimenTypeId });

            List<ExpertRuleActionReturnModel> ruleActions = new List<ExpertRuleActionReturnModel>();

            foreach (var rule in rules)
            {
                sql = @"select * from expertrulecondition where expertruleid = @ExpertRuleId and antibioticid = @AntibioticId and testmethodid = @TestMethodId and susceptibilityid = @SusceptibilityId and specialconsiderationid = @SpecialConsiderationId";

                var conditionsToUse = await connect.QueryAsync<ExpertRuleConditionReturnModel>(sql, new
                {
                    ExpertRuleId = rule.Id,
                    AntibioticId = antibioticid,
                    TestMethodId = testmethodid,
                    SusceptibilityId = susceptibilityid,
                    SpecialConsiderationId = specialconsiderationid
                });

                if (conditionsToUse.Any())
                {
                    var actionSql = @"select * from expertruleaction where expertruleid = @ExpertRuleId";
                    var conditionActions = await connect.QueryAsync<ExpertRuleActionReturnModel>(actionSql, new { ExpertRuleId = rule.Id });

                    if (conditionActions.Any())
                    {
                        foreach (var action in conditionActions)
                        {
                            action.ExpertRuleName = rule.ExpertRuleName;
                            action.RuleText = rule.RuleText;
                            action.SourceId = guidelinesId;
                        }
                        ruleActions.AddRange(conditionActions);
                    }
                    else
                    {
                        var alertAction = new ExpertRuleActionReturnModel
                        {
                            ExpertRuleName = rule.ExpertRuleName,
                            RuleText = rule.RuleText,
                            SourceId = guidelinesId
                        };
                        ruleActions.Add(alertAction);
                    }
                }
            }

            return ruleActions;
        }
    }
}