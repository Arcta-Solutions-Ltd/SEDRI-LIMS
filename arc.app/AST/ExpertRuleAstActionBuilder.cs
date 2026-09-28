using arc.app.ExpertRule;
using arc.common;
using arc.common.Models.AST;
using arc.common.Models.Coding;
using arc.common.Utils;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpertRuleModel = arc.domain.Coding.ExpertRule;

namespace arc.app.AST;

/// <summary>
/// Expands antibiotic-group expert rule actions into per-antibiotic AST rows, then filters all rows
/// (single-antibiotic and expanded group members) to antibiotics allowed by the culture laboratory.
/// </summary>
public class ExpertRuleAstActionBuilder : IExpertRuleAstActionBuilder
{
    private readonly IExpertRuleRepository _expertRuleRepository;
    private readonly IMapWithList<ExpertRuleActionReturnModel, ASTRowModel> _expertRuleMapper;
    private readonly ILogger<ExpertRuleAstActionBuilder> _logger;

    public ExpertRuleAstActionBuilder(
        IExpertRuleRepository expertRuleRepository,
        IMapWithList<ExpertRuleActionReturnModel, ASTRowModel> expertRuleMapper,
        ILogger<ExpertRuleAstActionBuilder> logger)
    {
        _expertRuleRepository = expertRuleRepository;
        _expertRuleMapper = expertRuleMapper;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<List<ASTRowModel>> BuildActionRowsAsync(
        string cultureId,
        ASTCultureModel cultureDetails,
        ExpertRuleModel rule,
        int testMethod,
        IReadOnlySet<int>? allowedLaboratoryAntibioticIds)
    {
        if (rule.RuleActionGrid is not { Count: > 0 })
        {
            return [];
        }

        if (allowedLaboratoryAntibioticIds != null && allowedLaboratoryAntibioticIds.Count == 0)
        {
            _logger.LogInformation(
                "ExpertRuleAstActionBuilder: CultureId={CultureId}, LaboratoryId={LaboratoryId}, RuleId={RuleId}, RuleName={RuleName}. Laboratory has no allowed antibiotics (no antibiotic groups configured or groups could not be resolved); suppressing all expert rule actions for AST display.",
                cultureId,
                cultureDetails.LaboratoryId,
                rule.RuleId,
                rule.ExpertRuleName);
            return [];
        }

        var expanded = await ExpandActionRowsAsync(cultureId, cultureDetails, rule, testMethod);

        if (allowedLaboratoryAntibioticIds == null)
        {
            return expanded;
        }

        var filtered = FilterByAllowedLaboratoryAntibiotics(expanded, allowedLaboratoryAntibioticIds);

        if (filtered.Count == 0 && expanded.Count > 0)
        {
            _logger.LogInformation(
                "ExpertRuleAstActionBuilder: CultureId={CultureId}, RuleId={RuleId}, RuleName={RuleName}. Expanded {ExpandedCount} action row(s) but none matched the laboratory allowed antibiotic list.",
                cultureId,
                rule.RuleId,
                rule.ExpertRuleName,
                expanded.Count);
        }
        else if (filtered.Count == 0 && rule.RuleActionGrid.Count > 0)
        {
            _logger.LogInformation(
                "ExpertRuleAstActionBuilder: CultureId={CultureId}, RuleId={RuleId}, RuleName={RuleName}. Rule had {ActionCount} configured action(s) but expansion produced no rows.",
                cultureId,
                rule.RuleId,
                rule.ExpertRuleName,
                rule.RuleActionGrid.Count);
        }
        else if (filtered.Count < expanded.Count)
        {
            _logger.LogInformation(
                "ExpertRuleAstActionBuilder: CultureId={CultureId}, RuleId={RuleId}, RuleName={RuleName}. Laboratory antibiotic filter kept {KeptCount} of {ExpandedCount} expanded action row(s).",
                cultureId,
                rule.RuleId,
                rule.ExpertRuleName,
                filtered.Count,
                expanded.Count);
        }

        return filtered;
    }

    /// <summary>
    /// Expands all configured actions to per-antibiotic AST rows without applying the laboratory allow list.
    /// Group actions expand to every member of the target antibiotic group via <c>antibioticcoding</c>.
    /// </summary>
    private async Task<List<ASTRowModel>> ExpandActionRowsAsync(
        string cultureId,
        ASTCultureModel cultureDetails,
        ExpertRuleModel rule,
        int testMethod)
    {
        var storedGroupIds = rule.RuleActionGrid
            .Where(a => (a.AntibioticId ?? 0) <= 0 && a.AntibioticGroupId.HasValue && a.AntibioticGroupId.Value > 0)
            .Select(a => a.AntibioticGroupId!.Value)
            .Where(id => id != AntibioticGroupListIds.MasterListItemId)
            .Distinct()
            .ToList();

        var membersByGroupId = new Dictionary<int, List<int>>();
        if (storedGroupIds.Count > 0)
        {
            var memberPairs = await _expertRuleRepository.GetAntibioticIdsByGroupIdsAsync(storedGroupIds);
            foreach (var pair in memberPairs)
            {
                if (!membersByGroupId.TryGetValue(pair.GroupId, out var list))
                {
                    list = [];
                    membersByGroupId[pair.GroupId] = list;
                }

                list.Add(pair.Id);
            }
        }

        var actions = new List<ASTRowModel>();

        foreach (var action in rule.RuleActionGrid)
        {
            var antibioticId = action.AntibioticId ?? 0;
            var storedGroupId = action.AntibioticGroupId;

            if (antibioticId > 0)
            {
                actions.Add(MapActionRow(cultureDetails, rule, action, testMethod, antibioticId));
                continue;
            }

            if (!storedGroupId.HasValue || storedGroupId.Value <= 0)
            {
                continue;
            }

            if (storedGroupId.Value == AntibioticGroupListIds.MasterListItemId)
            {
                _logger.LogWarning(
                    "ExpertRuleAstActionBuilder: CultureId={CultureId}, RuleId={RuleId}, ActionId={ActionId}, StoredGroupId={StoredGroupId}. Master is not a valid group action target.",
                    cultureId,
                    rule.RuleId,
                    action.ActionId,
                    storedGroupId.Value);
                continue;
            }

            if (!membersByGroupId.TryGetValue(storedGroupId.Value, out var memberIds) || memberIds.Count == 0)
            {
                _logger.LogWarning(
                    "ExpertRuleAstActionBuilder: CultureId={CultureId}, RuleId={RuleId}, ActionId={ActionId}, StoredGroupId={StoredGroupId}. No member antibiotics found for group action in antibioticcoding.",
                    cultureId,
                    rule.RuleId,
                    action.ActionId,
                    storedGroupId.Value);
                continue;
            }

            var distinctMembers = memberIds.Distinct().OrderBy(id => id).ToList();
            _logger.LogInformation(
                "ExpertRuleAstActionBuilder: CultureId={CultureId}, RuleId={RuleId}, ActionId={ActionId}, StoredGroupId={StoredGroupId}, MemberCount={MemberCount}. Expanded group action to per-antibiotic AST rows.",
                cultureId,
                rule.RuleId,
                action.ActionId,
                storedGroupId.Value,
                distinctMembers.Count);

            foreach (var memberId in distinctMembers)
            {
                actions.Add(MapActionRow(cultureDetails, rule, action, testMethod, memberId));
            }
        }

        return actions;
    }

    /// <summary>
    /// Keeps only rows whose <see cref="ASTRowModel.Antibiotic"/> id is in the laboratory allow list.
    /// Applies equally to single-antibiotic actions and rows produced from group expansion.
    /// </summary>
    private static List<ASTRowModel> FilterByAllowedLaboratoryAntibiotics(
        IReadOnlyList<ASTRowModel> rows,
        IReadOnlySet<int> allowedLaboratoryAntibioticIds)
    {
        if (rows == null || rows.Count == 0)
        {
            return [];
        }

        return rows
            .Where(r => r.Antibiotic.HasValue
                && r.Antibiotic.Value > 0
                && allowedLaboratoryAntibioticIds.Contains(r.Antibiotic.Value))
            .ToList();
    }

    private ASTRowModel MapActionRow(
        ASTCultureModel cultureDetails,
        ExpertRuleModel rule,
        RuleActionGridModel action,
        int testMethod,
        int antibioticId)
    {
        var erm = new ExpertRuleActionReturnModel
        {
            ExpertRuleId = rule.RuleId,
            ExpertRuleName = rule.ExpertRuleName,
            RuleText = rule.RuleText,
            AntibioticId = antibioticId,
            AntibioticGroupId = null,
            SusceptibilityId = action.SusceptibilityId,
            DisplayOnReport = string.IsNullOrWhiteSpace(action.DisplayOnReport) ? null : action.DisplayOnReport,
            SourceId = rule.SpecificationGuidelinesId
        };
        var row = _expertRuleMapper.Map(erm);
        row.TestMethod = testMethod;
        row.OrganismId = cultureDetails.OrganismId;
        row.SpecimenTypeId = cultureDetails.SpecimenTypeId;
        row.LaboratoryId = cultureDetails.LaboratoryId;
        row.IsPrintOnReportOnlyExpertAction = antibioticId > 0 && action.SusceptibilityId == 0;
        return row;
    }
}
