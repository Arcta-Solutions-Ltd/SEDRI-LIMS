using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
using arc.domain.Coding;
using arc.domain.Alert;

namespace arc.data.Coding
{
    internal class EditExpertRuleQuery : IQueryReturningType<ExpertRule>
    {
        /// <summary>
        /// Loads an expert rule by ID with its conditions, test conditions, and actions for the edit form.
        /// </summary>
        /// <param name="connect">Active database connection.</param>
        /// <param name="queryFilters">Must contain "id" with the expert rule ID.</param>
        /// <returns>The expert rule with nested grids populated.</returns>
        public async Task<ExpertRule> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("id");

            var sql = @"select e.Id as RuleId, e.ExpertRuleName, e.RuleText, e.CombinationRule, e.SpecificationId, e.Enabled, e.AlertOnRule, e.TagId, od.name as Order, f.name as Family, o.id as OrganismId, e.OrderId as OrderId, e.FamilyId as FamilyId, e.OrgGroupCodingId as OrgGroupCodingId, l.value as OrganismGroup, et.specimentypestoinclude, ert.specimentypestoexclude from ExpertRule e
                        left outer join ordercat od on od.id = e.orderid
                        left outer join family f on f.id = e.familyid
                        left outer join organism o on o.id = e.organismid
                        left outer join listitem l on l.id = e.orggroupcodingid
                        left outer join
			            (select expertruleid, STRING_AGG(cast(specimentypeid as varchar(7)), ',') as specimentypestoinclude from expertrulespecimentype where included = true group by expertruleid) et
			            on et.expertruleid = e.id 
                        left outer join
			            (select expertruleid, STRING_AGG(cast(specimentypeid as varchar(7)), ',') as specimentypestoexclude from expertrulespecimentype where included = false group by expertruleid) ert
			            on ert.expertruleid = e.id
                        where e.Id = @Id";

            var rule = await connect.QueryFirstAsync<ExpertRule>(sql, new { Id = id });

            sql = @"select Id as ConditionId, ExpertRuleId, LastModifiedDate, AntibioticId, AntibioticGroupId, TestMethodId, SusceptibilityId, SpecialConsiderationId, StartVal, EndVal from expertrulecondition where expertruleid = @ExpertRuleId";
            var conditionLines = await connect.QueryAsync<RuleConditionGridModel>(sql, new { ExpertRuleId = id });
            rule.RuleConditionGrid = conditionLines.ToList();

            sql = @"select Id as TestConditionId, ExpertRuleId, LastModifiedDate, TestName as Test, FieldName as Field, Comparison, CompValue as StringValue, CompValue as NumberValue, CompValue as ListValue from expertruletestcondition where expertruleid = @ExpertRuleId";
            var testConditionLines = await connect.QueryAsync<RuleTestConditionGridModel>(sql, new { ExpertRuleId = id });
            rule.RuleTestConditionGrid = testConditionLines.ToList();

            sql = @"select Id, Id as ActionId, ExpertRuleId, AntibioticId, AntibioticGroupId, SusceptibilityId, DisplayOnReport, LastModifiedDate from expertruleaction where expertruleid = @ExpertRuleId";
            var actionLines = await connect.QueryAsync<RuleActionGridModel>(sql, new { ExpertRuleId = id });
            rule.RuleActionGrid = actionLines.ToList();

            return rule;
        }
    }
}