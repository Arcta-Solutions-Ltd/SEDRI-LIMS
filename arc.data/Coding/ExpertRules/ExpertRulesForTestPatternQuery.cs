using arc.common.Models.Coding;
using arc.common.Models.AST;
using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace arc.data.Coding
{
    internal class ExpertRulesForTestPatternQuery : IQueryReturningType<List<ExpertRuleActionReturnModel>>
    {
        /// <summary>
        /// Retrieves unconditional (intrinsic resistance) expert rule actions for a test pattern. Matches rules scoped to the isolate organism
        /// (<c>expertrule.organismid</c>) or to a custom organism group when the organism appears in <c>organismcoding</c> for that group
        /// (<c>expertrule.orggroupcodingid</c>). Also filters by specification (via guidelinesid) and specimen type.
        /// </summary>
        /// <param name="connect">Active database connection.</param>
        /// <param name="queryFilters">Must contain: organismid, sourceid (guidelines listitem id), specimentypeid.</param>
        /// <returns>List of expert rule actions for intrinsic resistance display.</returns>
        public async Task<List<ExpertRuleActionReturnModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var organismid = queryFilters.GetIntegerValue("organismid");
            var specimentypeid = queryFilters.GetIntegerValue("specimentypeid");

            var sql = @"select r.* from expertrule r
                inner join specification s on r.specificationid = s.id
                where (
                    r.organismid = @OrganismId
                    or (
                        r.orggroupcodingid is not null
                        and r.orggroupcodingid <> 0
                        and exists (
                            select 1 from organismcoding oc
                            where oc.codingid = r.orggroupcodingid and oc.organismid = @OrganismId
                        )
                    )
                )
                and exists (select 1 from expertruleaction a where a.expertruleid = r.id)
                and not exists (select 1 from expertrulecondition c where c.expertruleid = r.id)
                and (not exists (select 1 from expertrulespecimentype ert where ert.expertruleid = r.id) or exists (select 1 from expertrulespecimentype ert where ert.expertruleid = r.id and ert.specimentypeid = @SpecimenTypeId and ert.included = true))";
            var rules = await connect.QueryAsync<ExpertRuleDetailsModel>(sql, new { OrganismId = organismid, SpecimenTypeId = specimentypeid });

            List<ExpertRuleActionReturnModel> ruleActions = new List<ExpertRuleActionReturnModel>();

            foreach (var rule in rules)
            {
                sql = @"select * from expertruleaction where expertruleid = @ExpertRuleId";
                var actions = await connect.QueryAsync<ExpertRuleActionReturnModel>(sql, new { ExpertRuleId = rule.Id });
                foreach (var action in actions)
                {
                    action.ExpertRuleName = rule.ExpertRuleName;
                    action.RuleText = rule.RuleText;
                }

                ruleActions.AddRange(actions);
            }

            return ruleActions;
        }
    }
}