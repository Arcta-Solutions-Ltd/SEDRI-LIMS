using arc.app.Common;
using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Data query that loads expert rule actions for a given expert rule.
/// Resolves antibiotic, antibiotic group, susceptibility, and display-on-report to display text.
/// </summary>
internal class ExpertRuleActionListByExpertRuleIdQuery : IQueryReturningType<List<ExpertRuleActionListModel>>
{
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Creates the query with logging for unresolved antibiotic group display names.
    /// </summary>
    /// <param name="logWriter">Logger used when a stored group id does not resolve to display text.</param>
    public ExpertRuleActionListByExpertRuleIdQuery(ILogWriter logWriter)
    {
        _logWriter = logWriter;
    }

    /// <summary>
    /// Executes the query and returns the list of actions for the given expert rule.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Filter config; expects "ExpertRuleId" with the expert rule ID.</param>
    /// <returns>List of action rows with display text for antibiotic, antibiotic group, susceptibility, and display-on-report.</returns>
    public async Task<List<ExpertRuleActionListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var expertRuleId = queryFilters.GetIntegerValue("ExpertRuleId");

        // AntibioticGroupId may store either a listitem id (optionsName antibioticgroup / GetListValuesQuery)
        // or an antibioticgroup.id from environments that seed numeric group ids differently (e.g. Cypress PostgreSetUpDataScript).
        var sql = """
            select
                a.Id,
                a.antibioticid,
                a.antibioticgroupid,
                case when coalesce(a.antibioticgroupid, 0) > 0 then null else ab.antibioticname end as antibioticdisplay,
                COALESCE(ag_li.value, ag_tab.name) as antibioticgroupdisplay,
                li2.value as susceptibilityname,
                a.displayonreport
            from expertruleaction a
            left join antibiotic ab on ab.id = a.antibioticid
            left join listitem ag_li on ag_li.id = a.antibioticgroupid
            left join antibioticgroup ag_tab on ag_tab.id = a.antibioticgroupid
            left join listitem li2 on li2.id = a.susceptibilityid
            where a.expertruleid = @expertRuleId
            order by a.id
            """;

        var rawResults = (await connect.QueryAsync(sql, new { expertRuleId })).ToList();
        var results = new List<ExpertRuleActionListModel>();

        foreach (dynamic row in rawResults)
        {
            var model = new ExpertRuleActionListModel
            {
                Id = row.id,
                AntibioticDisplay = row.antibioticdisplay,
                AntibioticGroupDisplay = row.antibioticgroupdisplay,
                SusceptibilityName = row.susceptibilityname,
                DisplayOnReport = row.displayonreport
            };
            results.Add(model);

            var antibioticId = row.antibioticid as int?;
            var groupId = row.antibioticgroupid as int?;
            if (antibioticId.HasValue && antibioticId.Value > 0 &&
                groupId.HasValue && groupId.Value > 0)
            {
                _logWriter.LogInfo(
                    $"Expert rule action list: action id {model.Id} (expert rule {expertRuleId}) has both antibioticid={antibioticId.Value} and antibioticgroupid={groupId.Value}.",
                    nameof(ExpertRuleActionListByExpertRuleIdQuery),
                    nameof(ExecuteAsync));
            }

            if (groupId.HasValue && groupId.Value > 0 && string.IsNullOrWhiteSpace(model.AntibioticGroupDisplay))
            {
                _logWriter.LogInfo(
                    $"Expert rule action list: antibiotic group id {groupId.Value} on action id {model.Id} (expert rule {expertRuleId}) did not resolve to display text.",
                    nameof(ExpertRuleActionListByExpertRuleIdQuery),
                    nameof(ExecuteAsync));
            }
        }

        return results;
    }
}
