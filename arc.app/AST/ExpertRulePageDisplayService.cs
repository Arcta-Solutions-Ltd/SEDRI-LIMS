using arc.app.ExpertRule;
using arc.app.Tests;
using arc.common;
using arc.common.Models.AST;
using arc.common.Models.Coding;
using arc.common.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Tests;
using ExpertRuleModel = arc.domain.Coding.ExpertRule;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.AST;

/// <summary>
/// Computes <see cref="ExpertRuleGroupModel"/>, <see cref="ExpertRulePageDisplayResult.ExpertRuleResults"/>, and
/// <see cref="ExpertRulePageDisplayResult.ExpertRuleCommentAlerts"/> for the AST screen
/// using <see cref="ExpertRuleDisplayValidityEvaluator"/> and organism- and guideline-scoped rules from the repository.
/// </summary>
public class ExpertRulePageDisplayService : IExpertRulePageDisplayService
{
    private readonly IExpertRuleRepository _expertRuleRepository;
    private readonly ITestRepository _testRepository;
    private readonly IExpertRuleAstActionBuilder _expertRuleAstActionBuilder;
    private readonly IConvertJsonStructureToKeyValuePair _pairConverter;
    private readonly ILogger<ExpertRulePageDisplayService> _logger;

    public ExpertRulePageDisplayService(
        IExpertRuleRepository expertRuleRepository,
        ITestRepository testRepository,
        IExpertRuleAstActionBuilder expertRuleAstActionBuilder,
        IConvertJsonStructureToKeyValuePair pairConverter,
        ILogger<ExpertRulePageDisplayService> logger)
    {
        _expertRuleRepository = expertRuleRepository;
        _testRepository = testRepository;
        _expertRuleAstActionBuilder = expertRuleAstActionBuilder;
        _pairConverter = pairConverter;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ExpertRulePageDisplayResult> BuildDisplayAsync(
        string cultureId,
        ASTCultureModel cultureDetails,
        IEnumerable<ASTRowModel> diskResults,
        IEnumerable<ASTRowModel> micResults,
        int persistedAstRowCount,
        IReadOnlyCollection<int> savedExpertRuleIdsFromPersistence)
    {
        // persistedAstRowCount / savedExpertRuleIdsFromPersistence: kept on the API for callers; ApplyRule defaults true for groups with actions.

        var diskList = diskResults?.ToList() ?? new List<ASTRowModel>();
        var micList = micResults?.ToList() ?? new List<ASTRowModel>();
        var flattenedAstForRules = ExpertRuleDisplayValidityEvaluator.FlattenAstRows(diskList.Concat(micList));
        var guidelineIdsCsv = string.Join(",", flattenedAstForRules.Where(r => r.Guidelines != 0).Select(r => r.Guidelines).Distinct());
        var expertRuleQueryFilters = new QueryFilterConfig()
            .AddString("OrganismId", cultureDetails.OrganismId.ToString());
        if (!string.IsNullOrWhiteSpace(guidelineIdsCsv))
        {
            expertRuleQueryFilters
                .AddString("RestrictGuidelines", "true")
                .AddString("GuidelineIds", guidelineIdsCsv);
        }
        var rules = await _expertRuleRepository.RetrieveExpertRulesAsync(expertRuleQueryFilters);
        var cultureTestsForRules = await _testRepository.GetTestsForCultureAsync(cultureId);
        var cultureTestFieldLines = BuildCultureTestFieldLines(cultureTestsForRules);
        var validityLookups = await BuildValidityLookupsAsync(flattenedAstForRules, rules);
        var validityContext = new ExpertRuleDisplayValidityContext
        {
            Rules = rules,
            AstRows = flattenedAstForRules,
            CultureTestFields = cultureTestFieldLines,
            SpecimenTypeId = cultureDetails.SpecimenTypeId,
            AntibioticIdToCodingIds = validityLookups
        };

        var rulesToUse = new List<ExpertRuleModel>();
        foreach (var rule in rules)
        {
            var invalidReason = ExpertRuleDisplayValidityEvaluator.GetInvalidDisplayReason(rule, validityContext);
            if (invalidReason != null)
            {
                _logger.LogInformation(
                    "ExpertRule display invalid: CultureId={CultureId}, OrganismId={OrganismId}, RuleId={RuleId}, RuleName={RuleName}, SpecificationGuidelinesId={SpecificationGuidelinesId}, Reason={Reason}",
                    cultureId,
                    cultureDetails.OrganismId,
                    rule.RuleId,
                    rule.ExpertRuleName,
                    rule.SpecificationGuidelinesId,
                    invalidReason);
                continue;
            }

            var hasGrid =
                (rule.RuleConditionGrid?.Any() ?? false) || (rule.RuleTestConditionGrid?.Any() ?? false) ||
                (rule.RuleActionGrid?.Any() ?? false);
            // Intrinsic guidance rule: organism-scoped, text-only (no condition/test/action grid). It has no triggering
            // AST line, so the AST screen shows it next to the organism name. Retain it when it carries guidance text.
            var isIntrinsicGuidance = !hasGrid && !string.IsNullOrWhiteSpace(rule.RuleText);
            if (hasGrid || isIntrinsicGuidance)
            {
                rulesToUse.Add(rule);
            }
        }

        var result = new ExpertRulePageDisplayResult();
        var allowedLaboratoryAntibioticIds =
            await _expertRuleRepository.GetAllowedLaboratoryAntibioticIdsAsync(cultureDetails.LaboratoryId);

        foreach (var rule in rulesToUse)
        {
            var triggers = ExpertRuleDisplayValidityEvaluator.GetTriggeringAstRows(rule, validityContext);
            _logger.LogInformation(
                "ExpertRule display valid: CultureId={CultureId}, OrganismId={OrganismId}, RuleId={RuleId}, RuleName={RuleName}, HasActions={HasActions}, TriggerCount={TriggerCount}, Triggers=[{Triggers}]",
                cultureId,
                cultureDetails.OrganismId,
                rule.RuleId,
                rule.ExpertRuleName,
                rule.RuleActionGrid is { Count: > 0 },
                triggers.Count,
                string.Join(";", triggers.Select(t => FormatTriggerLogEntry(t, flattenedAstForRules))));

            if (rule.RuleActionGrid is not { Count: > 0 })
            {
                result.ExpertRuleCommentAlerts.Add(new ExpertRuleCommentAlertModel
                {
                    RuleId = rule.RuleId,
                    RuleName = rule.ExpertRuleName,
                    RuleText = rule.RuleText,
                    Triggers = triggers.ToList()
                });
                continue;
            }

            var testMethod = ResolveExpertRuleAstTestMethod(rule);
            var actions = await _expertRuleAstActionBuilder.BuildActionRowsAsync(
                cultureId,
                cultureDetails,
                rule,
                testMethod,
                allowedLaboratoryAntibioticIds);

            if (actions.Count == 0)
            {
                continue;
            }

            result.ExpertRuleGroups.Add(new ExpertRuleGroupModel
            {
                RuleId = rule.RuleId,
                RuleName = rule.ExpertRuleName,
                RuleText = rule.RuleText,
                ApplyRule = true,
                Actions = actions,
                Triggers = triggers.ToList()
            });
        }

        result.ExpertRuleResults = result.ExpertRuleGroups.SelectMany(g => g.Actions).ToList();
        EnrichExpertRuleResultRows(result.ExpertRuleResults, rules);

        var printOnlyActionCount = result.ExpertRuleResults.Count(r => r.IsPrintOnReportOnlyExpertAction);
        if (printOnlyActionCount > 0)
        {
            var printOnlyRuleIds = string.Join(",", result.ExpertRuleGroups
                .Where(g => g.Actions.Any(a => a.IsPrintOnReportOnlyExpertAction))
                .Select(g => g.RuleId));
            _logger.LogInformation(
                "ExpertRulePageDisplay print-on-report actions: CultureId={CultureId}, OrganismId={OrganismId}, PrintOnlyActionCount={Count}, RuleIds=[{RuleIds}]",
                cultureId, cultureDetails.OrganismId, printOnlyActionCount, printOnlyRuleIds);
        }

        var unsetDisplayOnReportActionCount = result.ExpertRuleResults.Count(r => string.IsNullOrWhiteSpace(r.IncludeOnReport));
        if (unsetDisplayOnReportActionCount > 0)
        {
            var unsetDisplayOnReportRuleIds = string.Join(",", result.ExpertRuleGroups
                .Where(g => g.Actions.Any(a => string.IsNullOrWhiteSpace(a.IncludeOnReport)))
                .Select(g => g.RuleId));
            _logger.LogInformation(
                "ExpertRulePageDisplay unset display-on-report actions: CultureId={CultureId}, OrganismId={OrganismId}, UnsetActionCount={Count}, RuleIds=[{RuleIds}]. These render an editable include-on-report toggle (defaulting to Yes) on the AST screen.",
                cultureId, cultureDetails.OrganismId, unsetDisplayOnReportActionCount, unsetDisplayOnReportRuleIds);
        }

        _logger.LogDebug(
            "ExpertRulePageDisplay: CultureId={CultureId}, OrganismId={OrganismId}, RuleGroupCount={RuleCount}, CommentAlertCount={CommentCount}, FlattenedAstRowCount={AstCount}",
            cultureId, cultureDetails.OrganismId, result.ExpertRuleGroups.Count, result.ExpertRuleCommentAlerts.Count, flattenedAstForRules.Count);

        if (result.ExpertRuleCommentAlerts.Count > 0)
        {
            var commentIds = string.Join(",", result.ExpertRuleCommentAlerts.Select(c => c.RuleId));
            _logger.LogInformation(
                "ExpertRulePageDisplay comment alerts: CultureId={CultureId}, OrganismId={OrganismId}, CommentAlertCount={Count}, RuleIds=[{RuleIds}]",
                cultureId, cultureDetails.OrganismId, result.ExpertRuleCommentAlerts.Count, commentIds);
        }

        LogDuplicateRuleNames(cultureId, cultureDetails.OrganismId, result);

        return result;
    }

    /// <summary>
    /// Formats a triggering AST line for Information logging, including MIC and zone diameter values (or blank).
    /// </summary>
    private static string FormatTriggerLogEntry(ExpertRuleTriggerModel trigger, IReadOnlyList<ASTRowModel> astRows)
    {
        var row = astRows.FirstOrDefault(r =>
            r.TestMethod == trigger.TestMethod
            && (r.Antibiotic ?? 0) == trigger.AntibioticId);
        var micStr = row?.Mic.HasValue == true ? row.Mic.Value.ToString() : "blank";
        var zdStr = row?.ZoneDiameter.HasValue == true ? row.ZoneDiameter.Value.ToString() : "blank";
        return $"tm={trigger.TestMethod},ab={trigger.AntibioticId},sc={trigger.SpecialConsiderationId},mic={micStr},zd={zdStr}";
    }

    /// <summary>
    /// Logs a Warning for each set of two or more displayed expert rules (action groups and/or comment alerts) that
    /// share the same <c>RuleName</c>. Duplicate display names are valid in the data but rely on the AST screen
    /// grouping by <c>RuleId</c> (never name); this log makes the situation visible on installed systems that cannot
    /// run in debug mode, listing the colliding rule ids by name.
    /// </summary>
    /// <param name="cultureId">Culture id for the page being built.</param>
    /// <param name="organismId">Organism id for the culture.</param>
    /// <param name="result">The built display result (groups and comment alerts).</param>
    private void LogDuplicateRuleNames(string cultureId, int organismId, ExpertRulePageDisplayResult result)
    {
        var named = result.ExpertRuleGroups
            .Select(g => (g.RuleName, g.RuleId))
            .Concat(result.ExpertRuleCommentAlerts.Select(c => (c.RuleName, c.RuleId)))
            .Where(x => !string.IsNullOrWhiteSpace(x.RuleName));

        var duplicates = named
            .GroupBy(x => x.RuleName)
            .Where(grp => grp.Select(x => x.RuleId).Distinct().Count() > 1);

        foreach (var grp in duplicates)
        {
            _logger.LogWarning(
                "ExpertRulePageDisplay duplicate rule name: CultureId={CultureId}, OrganismId={OrganismId}, RuleName={RuleName}, RuleIds=[{RuleIds}]. The AST screen groups by RuleId, so these render as separate rules.",
                cultureId,
                organismId,
                grp.Key,
                string.Join(",", grp.Select(x => x.RuleId).Distinct()));
        }
    }

    /// <summary>
    /// Loads antibiotic-to-group listitem id lookups used by <see cref="ExpertRuleDisplayValidityEvaluator"/>.
    /// </summary>
    private async Task<IReadOnlyDictionary<int, HashSet<int>>> BuildValidityLookupsAsync(
        IReadOnlyList<ASTRowModel> flattenedAstRows,
        IReadOnlyList<ExpertRuleModel> rules)
    {
        var antibioticIds = flattenedAstRows
            .Where(r => r.Antibiotic.HasValue && r.Antibiotic.Value > 0)
            .Select(r => r.Antibiotic!.Value)
            .Distinct()
            .ToList();

        var map = new Dictionary<int, HashSet<int>>();
        if (antibioticIds.Count == 0)
        {
            return map;
        }

        var pairs = await _expertRuleRepository.GetAntibioticGroupIdsByAntibioticIdsAsync(antibioticIds);
        foreach (var p in pairs)
        {
            if (!map.TryGetValue(p.Id, out var codingIds))
            {
                codingIds = [];
                map[p.Id] = codingIds;
            }

            codingIds.Add(p.GroupId);
        }

        return map;
    }

    private List<CultureTestFieldLine> BuildCultureTestFieldLines(IEnumerable<Test> tests)
    {
        var lines = new List<CultureTestFieldLine>();
        if (tests == null)
        {
            return lines;
        }

        foreach (var test in tests)
        {
            if (string.IsNullOrWhiteSpace(test.TestResults))
            {
                continue;
            }

            foreach (var kv in _pairConverter.Convert(test.TestResults, true))
            {
                lines.Add(new CultureTestFieldLine
                {
                    TestName = test.TestName,
                    Key = kv.Key,
                    Value = kv.Value,
                    TestId = test.Id,
                    CultureId = test.CultureId
                });
            }
        }

        return lines;
    }

    /// <summary>
    /// Resolves MIC (680) vs Disk (681) for expert-rule AST rows. The <c>expertrule</c> header is not loaded with a test method in
    /// <c>RetrieveExpertRulesAsync</c>; <see cref="RuleConditionGridModel.TestMethodId"/> on conditions carries it instead.
    /// Uses <see cref="ExpertRuleModel.TestMethodId"/> when it is already 680/681; otherwise derives from condition grid (single distinct value, or first matching condition when ambiguous).
    /// </summary>
    /// <param name="rule">Organism-scoped expert rule with optional condition grid.</param>
    /// <returns>680, 681, or 0 when unknown.</returns>
    private static int ResolveExpertRuleAstTestMethod(ExpertRuleModel rule)
    {
        if (rule.TestMethodId == ExpertRuleDisplayValidityEvaluator.MicTestMethodId
            || rule.TestMethodId == ExpertRuleDisplayValidityEvaluator.DiskTestMethodId)
        {
            return rule.TestMethodId;
        }

        var grid = rule.RuleConditionGrid;
        if (grid == null || grid.Count == 0)
        {
            return 0;
        }

        var validIds = grid
            .Select(c => c.TestMethodId)
            .Where(id => id == ExpertRuleDisplayValidityEvaluator.MicTestMethodId
                || id == ExpertRuleDisplayValidityEvaluator.DiskTestMethodId)
            .Distinct()
            .ToList();

        if (validIds.Count == 1)
        {
            return validIds[0];
        }

        if (validIds.Count > 1)
        {
            var first = grid.FirstOrDefault(c =>
                c.TestMethodId == ExpertRuleDisplayValidityEvaluator.MicTestMethodId
                || c.TestMethodId == ExpertRuleDisplayValidityEvaluator.DiskTestMethodId);
            return first?.TestMethodId ?? 0;
        }

        return 0;
    }

    /// <summary>
    /// Fills missing expert rule name and text on expert-rule result rows from the organism rule list.
    /// </summary>
    private static void EnrichExpertRuleResultRows(IList<ASTRowModel> rows, IReadOnlyList<ExpertRuleModel> organismRules)
    {
        if (rows == null || rows.Count == 0 || organismRules == null || organismRules.Count == 0)
        {
            return;
        }

        var byRuleId = organismRules.GroupBy(r => r.RuleId).ToDictionary(g => g.Key, g => g.First());

        foreach (var row in rows)
        {
            if (row.ExpertRuleId == 0 || !byRuleId.TryGetValue(row.ExpertRuleId, out var rule))
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(row.ExpertRuleName))
            {
                row.ExpertRuleName = rule.ExpertRuleName;
            }

            if (string.IsNullOrWhiteSpace(row.ExpertRuleText))
            {
                row.ExpertRuleText = rule.RuleText;
            }
        }
    }
}
