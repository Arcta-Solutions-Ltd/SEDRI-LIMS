using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Data query that loads expert rule conditions for a given expert rule.
/// Resolves antibiotic/antibiotic group, test method, susceptibility, and special consideration to display text.
/// </summary>
internal class ExpertRuleConditionListByExpertRuleIdQuery : IQueryReturningType<List<ExpertRuleConditionListModel>>
{
    /// <summary>
    /// Executes the query and returns the list of conditions for the given expert rule.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Filter config; expects "ExpertRuleId" with the expert rule ID.</param>
    /// <returns>List of condition rows with display text for all list fields.</returns>
    public async Task<List<ExpertRuleConditionListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var expertRuleId = queryFilters.GetIntegerValue("ExpertRuleId");

        // AntibioticGroupId may store either a listitem id (optionsName antibioticgroup / GetListValuesQuery)
        // or an antibioticgroup.id from environments that seed numeric group ids differently (e.g. Cypress PostgreSetUpDataScript).
        var sql = """
            select
                c.Id,
                COALESCE(a.antibioticname, ag_li.value, ag_tab.name) as antibioticdisplay,
                li1.value as testmethodname,
                li2.value as susceptibilityname,
                li3.value as specialconsiderationname,
                c.startval,
                c.endval
            from expertrulecondition c
            left join antibiotic a on a.id = c.antibioticid
            left join listitem ag_li on ag_li.id = c.antibioticgroupid
            left join antibioticgroup ag_tab on ag_tab.id = c.antibioticgroupid
            left join listitem li1 on li1.id = c.testmethodid
            left join listitem li2 on li2.id = c.susceptibilityid
            left join listitem li3 on li3.id = c.specialconsiderationid
            where c.expertruleid = @expertRuleId
            order by c.id
            """;

        var results = await connect.QueryAsync<ExpertRuleConditionListModel>(sql, new { expertRuleId });
        return results.ToList();
    }
}
