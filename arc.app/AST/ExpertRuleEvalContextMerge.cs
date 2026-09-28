using arc.app.ExpertRule;
using arc.common.Models.AST;
using System.Collections.Generic;

namespace arc.app.AST;

/// <summary>
/// Merges expert-rule evaluation context rows from <c>cultureastexpertruleevalcontext</c> into disk/MIC lists so
/// <see cref="ExpertRuleDisplayValidityEvaluator"/> and the AST screen see manual rows that were omitted from the
/// <c>AST</c> table when an expert rule suppressed them on save.
/// </summary>
public static class ExpertRuleEvalContextMerge
{
    /// <summary>
    /// When <see cref="ASTRowModel.TestMethod"/> is <c>0</c> (portal JSON may omit the property so it deserializes as default),
    /// sets it to disk (681) or MIC (680) for that section. Matches the fallback used when merging sidecar rows from
    /// <see cref="MergeIntoDiskMicLists"/> so <see cref="ExpertRuleDisplayValidityEvaluator.AstRowMatchesCondition"/> can match conditions.
    /// </summary>
    public static void NormalizeDefaultTestMethods(List<ASTRowModel> diskResults, List<ASTRowModel> micResults)
    {
        if (diskResults != null)
        {
            foreach (var row in diskResults)
            {
                NormalizeRowTestMethod(row, ExpertRuleDisplayValidityEvaluator.DiskTestMethodId);
            }
        }

        if (micResults != null)
        {
            foreach (var row in micResults)
            {
                NormalizeRowTestMethod(row, ExpertRuleDisplayValidityEvaluator.MicTestMethodId);
            }
        }
    }

    private static void NormalizeRowTestMethod(ASTRowModel row, int defaultTestMethodId)
    {
        if (row == null)
        {
            return;
        }

        if (row.TestMethod == 0)
        {
            row.TestMethod = defaultTestMethodId;
        }

        if (row.EmbeddedASTRows == null || row.EmbeddedASTRows.Count == 0)
        {
            return;
        }

        foreach (var child in row.EmbeddedASTRows)
        {
            NormalizeRowTestMethod(child, defaultTestMethodId);
        }
    }

    /// <summary>
    /// Appends sidecar rows when no existing row has the same <paramref name="testMethodId"/> and <paramref name="antibioticId"/>.
    /// Existing persisted AST rows win; duplicates are skipped (counts returned as skipped).
    /// </summary>
    /// <param name="diskResults">Disk section rows (681).</param>
    /// <param name="micResults">MIC section rows (680).</param>
    /// <param name="ctx">Persisted snapshot, or null.</param>
    /// <returns>Counts of rows added vs skipped per section.</returns>
    public static (int diskAdded, int diskSkipped, int micAdded, int micSkipped) MergeIntoDiskMicLists(
        List<ASTRowModel> diskResults,
        List<ASTRowModel> micResults,
        ExpertRuleEvalContextModel? ctx)
    {
        if (ctx == null)
        {
            return (0, 0, 0, 0);
        }

        var diskAdded = 0;
        var diskSkipped = 0;
        var micAdded = 0;
        var micSkipped = 0;

        if (ctx.DiskResults != null)
        {
            foreach (var row in ctx.DiskResults)
            {
                if (row == null)
                {
                    continue;
                }

                var tm = row.TestMethod != 0 ? row.TestMethod : ExpertRuleDisplayValidityEvaluator.DiskTestMethodId;
                if (HasAntibioticTestMethod(diskResults, tm, row.Antibiotic))
                {
                    diskSkipped++;
                    continue;
                }

                diskResults.Add(row);
                diskAdded++;
            }
        }

        if (ctx.MicResults != null)
        {
            foreach (var row in ctx.MicResults)
            {
                if (row == null)
                {
                    continue;
                }

                var tm = row.TestMethod != 0 ? row.TestMethod : ExpertRuleDisplayValidityEvaluator.MicTestMethodId;
                if (HasAntibioticTestMethod(micResults, tm, row.Antibiotic))
                {
                    micSkipped++;
                    continue;
                }

                micResults.Add(row);
                micAdded++;
            }
        }

        return (diskAdded, diskSkipped, micAdded, micSkipped);
    }

    /// <summary>
    /// True when a <strong>manual</strong> row (not <see cref="ASTRowModel.ExpertRuleLine"/>) already exists for the
    /// test method and antibiotic. Expert-rule result lines in <c>AST</c> share antibiotic ids with suppressed
    /// condition rows and must not block re-injecting the manual snapshot from the sidecar.
    /// </summary>
    private static bool HasAntibioticTestMethod(IReadOnlyList<ASTRowModel> list, int testMethodId, int? antibioticId)
    {
        foreach (var r in list)
        {
            if (r == null || r.ExpertRuleLine)
            {
                continue;
            }

            var tm = r.TestMethod != 0 ? r.TestMethod : testMethodId;
            if (tm == testMethodId && NullableIntEquals(r.Antibiotic, antibioticId))
            {
                return true;
            }
        }

        return false;
    }

    private static bool NullableIntEquals(int? a, int? b)
    {
        if (a == null && b == null)
        {
            return true;
        }

        if (a == null || b == null)
        {
            return false;
        }

        return a.Value == b.Value;
    }
}
