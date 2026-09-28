using arc.common.Models.Coding;
using arc.data.Utils;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding.ExpertRules;

internal class RetrieveExpertRulesQuery : IQueryReturningType<List<ExpertRule>>
{
    public async Task<List<ExpertRule>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var organismId = queryFilters.GetIntegerValue("organismid");

        var restrictGuidelines = false;
        var guidelineIds = Array.Empty<int>();
        if (queryFilters.TryGetStringValue("RestrictGuidelines", out var restrictStr)
            && string.Equals(restrictStr, "true", StringComparison.OrdinalIgnoreCase))
        {
            restrictGuidelines = true;
            if (queryFilters.TryGetStringValue("GuidelineIds", out var guidelineCsv) && !string.IsNullOrWhiteSpace(guidelineCsv))
            {
                guidelineIds = guidelineCsv
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(s => int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id) ? id : 0)
                    .Where(id => id != 0)
                    .Distinct()
                    .ToArray();
            }
        }

        var headerSql = """
            with organismvalues as (
                with org as (
                    select o.id as organismid, o.genusid, o.speciesid, o.subspeciesid, o.serotypeid, f.id as familyid, ord.id as orderid
                    from organism o
                    left outer join genus g on g.id = o.genusid
                    left outer join family f on f.id = g.familyid
                    left outer join ordercat ord on ord.id = f.orderid
                    where o.id = @OrganismId
                )
                select org.organismid, org1.id as genusid, org2.id as speciesid, org3.id as subspeciesid, org4.id as serotypeid, org.familyid, org.orderid
                from org
                left outer join organism org1 on org.genusid is not null and org1.genusid = org.genusid and org1.speciesid is null
                left outer join organism org2 on org.genusid is not null and org.speciesid is not null and org2.genusid = org.genusid and org2.speciesid = org.speciesid and org2.subspeciesid is null and org2.serotypeid is null
                left outer join organism org3 on org.genusid is not null and org.speciesid is not null and org.subspeciesid is not null and org3.genusid = org.genusid and org3.speciesid = org.speciesid and org3.subspeciesid = org.subspeciesid and org3.serotypeid is null
                left outer join organism org4 on org.genusid is not null and org.speciesid is not null and org.serotypeid is not null and org4.genusid = org.genusid and org4.speciesid = org.speciesid and org4.serotypeid = org.serotypeid and org4.subspeciesid is null
            ),
            applicable_rule as (
                select distinct id
                from (
                    select e.id
                    from expertrule e
                    inner join organismvalues org on e.organismid = org.organismid
                    union all
                    select e.id
                    from expertrule e
                    inner join organismvalues org on e.organismid = org.serotypeid
                    union all
                    select e.id
                    from expertrule e
                    inner join organismvalues org on e.organismid = org.subspeciesid
                    union all
                    select e.id
                    from expertrule e
                    inner join organismvalues org on e.organismid = org.speciesid
                    union all
                    select e.id
                    from expertrule e
                    inner join organismvalues org on e.organismid = org.genusid
                    union all
                    select e.id
                    from expertrule e
                    inner join organismvalues org on e.familyid = org.familyid and (e.organismid = 0 or e.organismid is null)
                    union all
                    select e.id
                    from expertrule e
                    inner join organismvalues org on e.orderid = org.orderid and (e.familyid = 0 or e.familyid is null)
                    union all
                    select e.id
                    from expertrule e
                    inner join organismcoding oc on e.orggroupcodingid = oc.CodingId
                    left outer join organism o on o.id = oc.organismid
                    left outer join genus g on o.genusid = g.id
                    left outer join family f on g.familyid = f.id
                    inner join organismvalues org on 1 = 1
                    where (
                        (oc.OrganismId = org.organismid
                            or (org.genusid is not null and oc.OrganismId = org.genusid)
                            or (org.speciesid is not null and oc.OrganismId = org.speciesid)
                            or (org.subspeciesid is not null and oc.OrganismId = org.subspeciesid)
                            or (org.serotypeid is not null and oc.OrganismId = org.serotypeid))
                        or (
                            (f.orderid = org.orderid or f.orderid = 0 or f.orderid is null) and
                            (g.familyid = org.familyid or g.familyid = 0 or g.familyid is null) and
                            (o.genusid = org.genusid or o.genusid = 0 or o.genusid is null) and
                            (o.speciesid = org.speciesid or o.speciesid = 0 or o.speciesid is null) and
                            (o.subspeciesid = org.subspeciesid or o.subspeciesid = 0 or o.subspeciesid is null) and
                            (o.serotypeid = org.serotypeid or o.serotypeid = 0 or o.serotypeid is null) and
                            (e.orggroupcodingid != 0) and (e.orggroupcodingid is not null)
                        )
                    )
                ) as rule_ids
            )
            select e.Id as RuleId, e.ExpertRuleName, e.RuleText, e.CombinationRule, e.SpecificationId, spec.guidelinesid as SpecificationGuidelinesId, e.Enabled, e.AlertOnRule, e.TagId, od.name as Order, f.name as Family, o.id as OrganismId, e.OrderId as OrderId, e.FamilyId as FamilyId, l.value as OrganismGroup, et.specimentypestoinclude, ert.specimentypestoexclude
            from expertrule e
            inner join applicable_rule ar on e.id = ar.id
            left outer join specification spec on spec.id = e.specificationid
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
            where (
                @restrict_guidelines = false
                or spec.guidelinesid = any(@guideline_ids)
                or (
                    not exists (select 1 from expertrulecondition c where c.expertruleid = e.id)
                    and not exists (select 1 from expertruletestcondition t where t.expertruleid = e.id)
                )
            )
            """;

        var rules = (await connect.QueryAsync<ExpertRule>(headerSql, new { OrganismId = organismId, restrict_guidelines = restrictGuidelines, guideline_ids = guidelineIds })).ToList();
        if (rules.Count == 0)
        {
            return rules;
        }

        var ruleIds = rules.Select(r => r.RuleId).Distinct().ToArray();

        var conditionSql = """
            select Id as ConditionId, ExpertRuleId, LastModifiedDate, AntibioticId, AntibioticGroupId, TestMethodId, SusceptibilityId, SpecialConsiderationId, StartVal, EndVal
            from expertrulecondition
            where expertruleid = any(@ruleIds)
            """;

        var testConditionSql = """
            select Id as TestConditionId, ExpertRuleId, LastModifiedDate, TestName as Test, FieldName as Field, Comparison, CompValue as StringValue, CompValue as NumberValue, CompValue as ListValue
            from expertruletestcondition
            where expertruleid = any(@ruleIds)
            """;

        var actionSql = """
            select Id as ActionId, ExpertRuleId, AntibioticId, AntibioticGroupId, SusceptibilityId, DisplayOnReport, LastModifiedDate
            from expertruleaction
            where expertruleid = any(@ruleIds)
            """;

        var param = new { ruleIds };

        var conditionLines = (await connect.QueryAsync<RuleConditionGridModel>(conditionSql, param)).ToList();
        var testConditionLines = (await connect.QueryAsync<RuleTestConditionGridModel>(testConditionSql, param)).ToList();
        var actionLines = (await connect.QueryAsync<RuleActionGridModel>(actionSql, param)).ToList();

        var conditionsByRule = conditionLines.GroupBy(c => c.ExpertRuleId).ToDictionary(g => g.Key, g => g.ToList());
        var testConditionsByRule = testConditionLines.GroupBy(t => t.ExpertRuleId).ToDictionary(g => g.Key, g => g.ToList());
        var actionsByRule = actionLines.GroupBy(a => a.ExpertRuleId).ToDictionary(g => g.Key, g => g.ToList());

        foreach (var rule in rules)
        {
            rule.RuleConditionGrid = conditionsByRule.TryGetValue(rule.RuleId, out var cg) ? cg : [];
            rule.RuleTestConditionGrid = testConditionsByRule.TryGetValue(rule.RuleId, out var tg) ? tg : [];
            rule.RuleActionGrid = actionsByRule.TryGetValue(rule.RuleId, out var ag) ? ag : [];
        }

        return rules;
    }
}
