using arc.common.ExtensionMethods;
using arc.common.Models.AST;
using arc.common.Models.Coding;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ExpertRuleModel = arc.domain.Coding.ExpertRule;

namespace arc.app.ExpertRule;

/// <summary>
/// Determines which organism-scoped expert rules are valid for display using AST rows (including per-row
/// <see cref="ASTRowModel.Guidelines"/> vs specification <c>SpecificationGuidelinesId</c>), isolate test field lines,
/// specimen type, and <c>CombinationRule</c> on the domain expert rule between AST and test condition grids.
/// </summary>
public static class ExpertRuleDisplayValidityEvaluator
{
    /// <summary>MIC test method id (matches AST handler partitioning).</summary>
    public const int MicTestMethodId = 680;

    /// <summary>Disk (zone) test method id.</summary>
    public const int DiskTestMethodId = 681;

    /// <summary>
    /// List item id used on AST/test-pattern rows as a sentinel for &quot;no special consideration&quot; (parent line).
    /// Stored expert-rule conditions may use <c>0</c> or <see cref="NoSpecialConsiderationSentinelId"/> for the same meaning; matching normalizes both.
    /// </summary>
    public const int NoSpecialConsiderationSentinelId = 973;

    /// <summary>
    /// Flattens MIC/disk rows and nested <see cref="ASTRowModel.EmbeddedASTRows"/>; optionally omits rows that represent expert-rule lines.
    /// </summary>
    public static List<ASTRowModel> FlattenAstRows(IEnumerable<ASTRowModel> roots, bool excludeExpertRuleLines = true)
    {
        var result = new List<ASTRowModel>();
        if (roots == null)
        {
            return result;
        }

        foreach (var root in roots)
        {
            Visit(root);
        }

        return result;

        void Visit(ASTRowModel row)
        {
            if (row == null)
            {
                return;
            }

            var isExpertLine = row.ExpertRuleLine || row.ExpertRuleId != 0;
            if (!excludeExpertRuleLines || !isExpertLine)
            {
                result.Add(row);
            }

            if (row.EmbeddedASTRows == null || row.EmbeddedASTRows.Count == 0)
            {
                return;
            }

            foreach (var child in row.EmbeddedASTRows)
            {
                Visit(child);
            }
        }
    }

    /// <summary>
    /// Returns rules that pass display validity for the given context.
    /// </summary>
    public static IReadOnlyList<ExpertRuleModel> FilterValidForDisplay(ExpertRuleDisplayValidityContext context)
    {
        if (context?.Rules == null || context.Rules.Count == 0)
        {
            return Array.Empty<ExpertRuleModel>();
        }

        var list = new List<ExpertRuleModel>();
        foreach (var rule in context.Rules)
        {
            if (IsValidForDisplay(rule, context))
            {
                list.Add(rule);
            }
        }

        return list;
    }

    /// <summary>
    /// Returns expert rules that pass display validity (same result as <see cref="FilterValidForDisplay"/>).
    /// </summary>
    public static IReadOnlyList<ExpertRuleModel> GetValidRulesForDisplay(ExpertRuleDisplayValidityContext context)
    {
        return FilterValidForDisplay(context);
    }

    /// <summary>
    /// Whether a single rule is valid for display. Order: enabled flag (when <see cref="ExpertRuleDisplayValidityContext.OnlyEnabledRules"/>),
    /// specimen include/exclude lists, non-zero <c>SpecificationGuidelinesId</c>, intrinsic rules (both condition grids empty),
    /// else AST conditions (including per-row <see cref="ASTRowModel.Guidelines"/> vs specification guideline;
    /// AND across rows, or OR when combination is not AND and only the AST grid has multiple rows),
    /// test conditions (same AND/OR pattern when only the test grid has multiple rows), then <c>CombinationRule</c> between the two groups.
    /// </summary>
    public static bool IsValidForDisplay(ExpertRuleModel rule, ExpertRuleDisplayValidityContext context)
    {
        return GetInvalidDisplayReason(rule, context) == null;
    }

    /// <summary>
    /// When the rule is not valid for display on the AST page, returns a short machine-readable reason for logging;
    /// otherwise <c>null</c>. Mirrors <see cref="IsValidForDisplay"/>.
    /// </summary>
    public static string? GetInvalidDisplayReason(ExpertRuleModel rule, ExpertRuleDisplayValidityContext context)
    {
        if (rule == null || context == null)
        {
            return "NullRuleOrContext";
        }

        if (context.OnlyEnabledRules && !string.Equals(rule.Enabled, "Yes", StringComparison.OrdinalIgnoreCase))
        {
            return "Disabled";
        }

        var specimenFailure = GetSpecimenScopeFailureReason(rule, context.SpecimenTypeId);
        if (specimenFailure != null)
        {
            return specimenFailure;
        }

        if (rule.SpecificationGuidelinesId == 0)
        {
            return "NoSpecificationGuidelines";
        }

        var astRows = context.AstRows ?? Array.Empty<ASTRowModel>();
        var fields = context.CultureTestFields ?? Array.Empty<CultureTestFieldLine>();

        var condGrid = rule.RuleConditionGrid ?? [];
        var testGrid = rule.RuleTestConditionGrid ?? [];

        // Intrinsic resistance: actions only — organism/specimen scope is sufficient; no AST rows required.
        if (condGrid.Count == 0 && testGrid.Count == 0)
        {
            return null;
        }

        var specGuidelinesId = rule.SpecificationGuidelinesId;
        var isAstTestAnd = IsAstTestCombinationAnd(rule.CombinationRule, context.CombinationRuleAndListItemId);
        var astSingleGroupOr = !isAstTestAnd && testGrid.Count == 0 && condGrid.Count > 1;
        var testSingleGroupOr = !isAstTestAnd && condGrid.Count == 0 && testGrid.Count > 1;

        var astSatisfied = astSingleGroupOr
            ? AstConditionGroupSatisfiedOr(condGrid, astRows, context.AntibioticIdToCodingIds, specGuidelinesId)
            : AstConditionGroupSatisfied(condGrid, astRows, context.AntibioticIdToCodingIds, specGuidelinesId);
        var testSatisfied = testSingleGroupOr
            ? TestConditionGroupSatisfiedOr(testGrid, fields)
            : TestConditionGroupSatisfied(testGrid, fields);

        if (CombineGroups(astSatisfied, testSatisfied, condGrid.Count, testGrid.Count, rule.CombinationRule, context.CombinationRuleAndListItemId))
        {
            return null;
        }

        if (condGrid.Count > 0 && !astSatisfied)
        {
            if (astSingleGroupOr)
            {
                return $"AstConditions(Or): no condition matched any AST row, AstRowCount={astRows.Count}";
            }

            var failedIndex = FindFirstUnmatchedConditionIndex(condGrid, astRows, context.AntibioticIdToCodingIds, specGuidelinesId);
            return $"AstConditions: no row matched condition index {failedIndex}, AstRowCount={astRows.Count}";
        }

        if (testGrid.Count > 0 && !testSatisfied)
        {
            if (testSingleGroupOr)
            {
                return $"TestConditions(Or): no test line matched any isolate field line, FieldLineCount={fields.Count}";
            }

            return $"TestConditions: isolate test field lines did not satisfy all rows, FieldLineCount={fields.Count}";
        }

        return $"Combination: CombinationRule={rule.CombinationRule ?? "(empty)"}, AstOk={astSatisfied}, TestOk={testSatisfied}";
    }

    /// <summary>
    /// Returns the AST lines (by id) that triggered a valid rule: every flattened AST row that matches at least one of the
    /// rule's AST condition rows. Returns an empty list for intrinsic rules (no condition grid) or rules that fired only via
    /// isolate test conditions. Results are de-duplicated by test method, antibiotic id, and (normalized) special consideration id.
    /// </summary>
    /// <param name="rule">A rule already determined valid for display.</param>
    /// <param name="context">Evaluation context with flattened AST rows and antibiotic-to-group lookup.</param>
    /// <returns>Distinct triggering AST line identities; never <c>null</c>.</returns>
    public static IReadOnlyList<ExpertRuleTriggerModel> GetTriggeringAstRows(ExpertRuleModel rule, ExpertRuleDisplayValidityContext context)
    {
        var triggers = new List<ExpertRuleTriggerModel>();
        if (rule == null || context == null)
        {
            return triggers;
        }

        var condGrid = rule.RuleConditionGrid ?? [];
        if (condGrid.Count == 0)
        {
            return triggers;
        }

        var astRows = context.AstRows ?? Array.Empty<ASTRowModel>();
        var specGuidelinesId = rule.SpecificationGuidelinesId;
        var seen = new HashSet<(int, int, int)>();

        foreach (var row in astRows)
        {
            var matchesAnyCondition = condGrid.Any(cond =>
                AstRowMatchesCondition(cond, row, context.AntibioticIdToCodingIds, specGuidelinesId));
            if (!matchesAnyCondition)
            {
                continue;
            }

            var antibioticId = row.Antibiotic ?? 0;
            var specialConsiderationId = NormalizeSpecialConsiderationIdForMatching(row.SpecialConsiderationId);
            var key = (row.TestMethod, antibioticId, specialConsiderationId);
            if (!seen.Add(key))
            {
                continue;
            }

            triggers.Add(new ExpertRuleTriggerModel
            {
                TestMethod = row.TestMethod,
                AntibioticId = antibioticId,
                SpecialConsiderationId = specialConsiderationId
            });
        }

        return triggers;
    }

    /// <summary>
    /// Returns a short reason when specimen type fails include/exclude filters; <c>null</c> when the rule passes specimen scope.
    /// </summary>
    private static string? GetSpecimenScopeFailureReason(ExpertRuleModel rule, int specimenTypeId)
    {
        var include = ParseIdCsv(rule.SpecimenTypesToInclude);
        if (include.Count > 0 && !include.Contains(specimenTypeId))
        {
            return $"Specimen: SpecimenTypeId={specimenTypeId} not in include list";
        }

        var exclude = ParseIdCsv(rule.SpecimenTypesToExclude);
        if (exclude.Count > 0 && exclude.Contains(specimenTypeId))
        {
            return $"Specimen: SpecimenTypeId={specimenTypeId} is excluded";
        }

        return null;
    }

    private static bool PassesSpecimenScope(ExpertRuleModel rule, int specimenTypeId)
    {
        return GetSpecimenScopeFailureReason(rule, specimenTypeId) == null;
    }

    /// <summary>
    /// True when some flattened AST row uses a non-zero guideline that matches the rule's specification guideline.
    /// </summary>
    private static bool HasAstRowForRuleGuidelines(IReadOnlyList<ASTRowModel> astRows, int specificationGuidelinesId)
    {
        return astRows.Any(r => r.Guidelines != 0 && r.Guidelines == specificationGuidelinesId);
    }

    private static HashSet<int> ParseIdCsv(string csv)
    {
        var set = new HashSet<int>();
        if (string.IsNullOrWhiteSpace(csv))
        {
            return set;
        }

        foreach (var part in csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
            {
                set.Add(id);
            }
        }

        return set;
    }

    /// <summary>
    /// First condition index (0-based) that no AST row satisfies.
    /// </summary>
    private static int FindFirstUnmatchedConditionIndex(
        IReadOnlyList<RuleConditionGridModel> conditions,
        IReadOnlyList<ASTRowModel> astRows,
        IReadOnlyDictionary<int, HashSet<int>> antibioticIdToCodingIds,
        int specificationGuidelinesId)
    {
        for (var i = 0; i < conditions.Count; i++)
        {
            var cond = conditions[i];
            if (!astRows.Any(row => AstRowMatchesCondition(cond, row, antibioticIdToCodingIds, specificationGuidelinesId)))
            {
                return i;
            }
        }

        return 0;
    }

    private static bool AstConditionGroupSatisfied(
        IReadOnlyList<RuleConditionGridModel> conditions,
        IReadOnlyList<ASTRowModel> astRows,
        IReadOnlyDictionary<int, HashSet<int>> antibioticIdToCodingIds,
        int specificationGuidelinesId)
    {
        if (conditions == null || conditions.Count == 0)
        {
            return true;
        }

        foreach (var cond in conditions)
        {
            var anyRowMatches = astRows.Any(row => AstRowMatchesCondition(cond, row, antibioticIdToCodingIds, specificationGuidelinesId));
            if (!anyRowMatches)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// True when at least one AST condition row matches some AST line (OR across condition rows).
    /// </summary>
    private static bool AstConditionGroupSatisfiedOr(
        IReadOnlyList<RuleConditionGridModel> conditions,
        IReadOnlyList<ASTRowModel> astRows,
        IReadOnlyDictionary<int, HashSet<int>> antibioticIdToCodingIds,
        int specificationGuidelinesId)
    {
        if (conditions == null || conditions.Count == 0)
        {
            return true;
        }

        foreach (var cond in conditions)
        {
            if (astRows.Any(row => AstRowMatchesCondition(cond, row, antibioticIdToCodingIds, specificationGuidelinesId)))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Maps special-consideration list ids to a canonical value for expert-rule matching:
    /// <c>0</c> and <see cref="NoSpecialConsiderationSentinelId"/> both mean &quot;no special consideration&quot; on a parent AST line.
    /// </summary>
    /// <param name="specialConsiderationId">Raw id from condition or AST row.</param>
    /// <returns>0 when the id denotes no special consideration; otherwise the id.</returns>
    private static int NormalizeSpecialConsiderationIdForMatching(int specialConsiderationId)
    {
        if (specialConsiderationId == NoSpecialConsiderationSentinelId)
        {
            return 0;
        }

        return specialConsiderationId;
    }

    /// <summary>
    /// Whether a single expert-rule condition row matches an AST row (guideline from rule specification, test method,
    /// antibiotic/group, susceptibility, special consideration, optional measurement range). Rows with <c>Guidelines == 0</c>
    /// never match; <paramref name="specificationGuidelinesId"/> must be non-zero.
    /// </summary>
    private static bool AstRowMatchesCondition(
        RuleConditionGridModel cond,
        ASTRowModel row,
        IReadOnlyDictionary<int, HashSet<int>> antibioticIdToCodingIds,
        int specificationGuidelinesId)
    {
        if (row.Guidelines == 0 || specificationGuidelinesId == 0 || row.Guidelines != specificationGuidelinesId)
        {
            return false;
        }

        if (cond.TestMethodId != 0 && row.TestMethod != cond.TestMethodId)
        {
            return false;
        }

        if (!AntibioticMatches(cond, row, antibioticIdToCodingIds))
        {
            return false;
        }

        if (cond.SusceptibilityId != 0 && row.TestResult != cond.SusceptibilityId)
        {
            return false;
        }

        var condSc = NormalizeSpecialConsiderationIdForMatching(cond.SpecialConsiderationId);
        var rowSc = NormalizeSpecialConsiderationIdForMatching(row.SpecialConsiderationId);
        if (condSc != 0 && rowSc != condSc)
        {
            return false;
        }

        if (!MeasuredValueInRange(cond, row))
        {
            return false;
        }

        return true;
    }

    private static bool AntibioticMatches(
        RuleConditionGridModel cond,
        ASTRowModel row,
        IReadOnlyDictionary<int, HashSet<int>> antibioticIdToCodingIds)
    {
        var hasAntibioticId = cond.AntibioticId.HasValue && cond.AntibioticId.Value > 0;
        var hasGroupId = cond.AntibioticGroupId.HasValue && cond.AntibioticGroupId.Value > 0;

        if (hasAntibioticId)
        {
            return row.Antibiotic.HasValue && row.Antibiotic.Value == cond.AntibioticId.Value;
        }

        if (hasGroupId)
        {
            if (antibioticIdToCodingIds == null || !row.Antibiotic.HasValue)
            {
                return false;
            }

            var groupListItemId = cond.AntibioticGroupId!.Value;
            return antibioticIdToCodingIds.TryGetValue(row.Antibiotic.Value, out var codingIds)
                && codingIds.Contains(groupListItemId);
        }

        return true;
    }

    /// <summary>
    /// Whether the row's MIC or zone diameter falls within the condition's inclusive <see cref="RuleConditionGridModel.StartVal"/> /
    /// <see cref="RuleConditionGridModel.EndVal"/> bounds. Blank sentinels (null, 0, -1) never satisfy a configured range.
    /// </summary>
    private static bool MeasuredValueInRange(RuleConditionGridModel cond, ASTRowModel row)
    {
        if (!cond.StartVal.HasValue && !cond.EndVal.HasValue)
        {
            return true;
        }

        if (!row.TryGetEnteredMeasurementForExpertRuleRange(out var v))
        {
            return false;
        }

        if (cond.StartVal.HasValue && v < cond.StartVal.Value)
        {
            return false;
        }

        if (cond.EndVal.HasValue && v > cond.EndVal.Value)
        {
            return false;
        }

        return true;
    }

    private static bool TestConditionGroupSatisfied(
        IReadOnlyList<RuleTestConditionGridModel> testConditions,
        IReadOnlyList<CultureTestFieldLine> fieldLines)
    {
        if (testConditions == null || testConditions.Count == 0)
        {
            return true;
        }

        foreach (var line in testConditions)
        {
            if (!TestLineMatches(line, fieldLines))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// True when at least one test condition line matches isolate test fields (OR across test condition rows).
    /// </summary>
    private static bool TestConditionGroupSatisfiedOr(
        IReadOnlyList<RuleTestConditionGridModel> testConditions,
        IReadOnlyList<CultureTestFieldLine> fieldLines)
    {
        if (testConditions == null || testConditions.Count == 0)
        {
            return true;
        }

        foreach (var line in testConditions)
        {
            if (TestLineMatches(line, fieldLines))
            {
                return true;
            }
        }

        return false;
    }

    private static bool TestLineMatches(RuleTestConditionGridModel line, IReadOnlyList<CultureTestFieldLine> fieldLines)
    {
        var testName = line.Test ?? line.TestName;
        var fieldName = line.Field ?? line.FieldName;
        var expected = line.StringValue ?? line.NumberValue ?? line.ListValue ?? line.CompValue ?? string.Empty;

        if (string.IsNullOrWhiteSpace(fieldName))
        {
            return false;
        }

        IEnumerable<CultureTestFieldLine> candidates = fieldLines;
        if (!string.IsNullOrWhiteSpace(testName))
        {
            candidates = fieldLines.Where(f =>
                string.Equals(f.TestName, testName, StringComparison.OrdinalIgnoreCase));
        }

        var comparison = (line.Comparison ?? string.Empty).Trim();

        foreach (var f in candidates)
        {
            if (!string.Equals(f.Key, fieldName, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (MatchesComparison(f.Value, expected, comparison))
            {
                return true;
            }
        }

        return false;
    }

    private static bool MatchesComparison(string actual, string expected, string comparison)
    {
        if (string.IsNullOrEmpty(comparison) || comparison == "=")
        {
            return string.Equals(actual, expected, StringComparison.Ordinal);
        }

        switch (comparison)
        {
            case ">":
            case "<":
            case ">=":
            case "<=":
                if (!decimal.TryParse(actual, NumberStyles.Any, CultureInfo.InvariantCulture, out var a))
                {
                    return false;
                }

                if (!decimal.TryParse(expected, NumberStyles.Any, CultureInfo.InvariantCulture, out var e))
                {
                    return false;
                }

                return comparison switch
                {
                    ">" => a > e,
                    "<" => a < e,
                    ">=" => a >= e,
                    "<=" => a <= e,
                    _ => false
                };
            default:
                return string.Equals(actual, expected, StringComparison.Ordinal);
        }
    }

    /// <summary>
    /// True when <paramref name="combinationRule"/> means AND between AST and test condition <em>groups</em> (list id or empty).
    /// </summary>
    private static bool IsAstTestCombinationAnd(string combinationRule, string andListItemId)
    {
        var cr = (combinationRule ?? string.Empty).Trim();
        return string.Equals(cr, andListItemId, StringComparison.OrdinalIgnoreCase)
            || string.IsNullOrWhiteSpace(cr);
    }

    private static bool CombineGroups(
        bool astGroupSatisfied,
        bool testGroupSatisfied,
        int astConditionCount,
        int testConditionCount,
        string combinationRule,
        string andListItemId)
    {
        var hasAst = astConditionCount > 0;
        var hasTest = testConditionCount > 0;

        if (!hasAst && !hasTest)
        {
            return true;
        }

        var astPart = !hasAst || astGroupSatisfied;
        var testPart = !hasTest || testGroupSatisfied;

        var isAnd = IsAstTestCombinationAnd(combinationRule, andListItemId);

        if (isAnd)
        {
            return astPart && testPart;
        }

        if (hasAst && hasTest)
        {
            return astGroupSatisfied || testGroupSatisfied;
        }

        return astPart && testPart;
    }
}
