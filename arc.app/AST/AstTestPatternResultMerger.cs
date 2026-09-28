using arc.common.ExtensionMethods;
using arc.common.Models.AST;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.AST;

/// <summary>
/// Merges existing manual AST results onto new test pattern lines when the user changes test pattern.
/// Pattern line structure wins; entered measurements and susceptibilities are copied by id-based line key.
/// </summary>
public static class AstTestPatternResultMerger
{
    /// <summary>Counts from <see cref="MergeExistingOntoPatternLines"/> for support logging.</summary>
    public sealed class MergeStats
    {
        public int PatternLineCount { get; init; }
        public int ExistingRowCount { get; init; }
        public int MergedCount { get; init; }
        public int UnmatchedExistingCount { get; init; }
    }

    /// <summary>
    /// For each pattern line, copy entered results from the first matching existing manual row.
    /// </summary>
    /// <param name="patternLines">Lines from the newly selected test pattern.</param>
    /// <param name="existingRows">Persisted or in-memory manual AST rows before the pattern change.</param>
    /// <returns>Merged pattern lines and merge statistics.</returns>
    public static (List<ASTRowModel> merged, MergeStats stats) MergeExistingOntoPatternLines(
        IReadOnlyList<ASTRowModel> patternLines,
        IReadOnlyList<ASTRowModel> existingRows)
    {
        var patternList = patternLines?.ToList() ?? new List<ASTRowModel>();
        var existingList = existingRows?.Where(r => r.IsMergeableManualLine()).ToList() ?? new List<ASTRowModel>();

        var existingByKey = new Dictionary<string, ASTRowModel>();
        foreach (var row in existingList)
        {
            var key = row.BuildLineMatchKey();
            if (key.Length > 0 && !existingByKey.ContainsKey(key))
            {
                existingByKey[key] = row;
            }
        }

        var mergedCount = 0;
        var merged = new List<ASTRowModel>(patternList.Count);
        foreach (var patternLine in patternList)
        {
            var key = patternLine.BuildLineMatchKey();
            if (key.Length > 0 && existingByKey.TryGetValue(key, out var existing))
            {
                merged.Add(MergeResultOntoPatternLine(patternLine, existing));
                mergedCount++;
            }
            else
            {
                merged.Add(patternLine);
            }
        }

        var matchedKeys = merged
            .Where(r => r.IsMergeableManualLine())
            .Select(r => r.BuildLineMatchKey())
            .ToHashSet();
        var unmatchedExisting = existingList.Count(r => !matchedKeys.Contains(r.BuildLineMatchKey()));

        var stats = new MergeStats
        {
            PatternLineCount = patternList.Count,
            ExistingRowCount = existingList.Count,
            MergedCount = mergedCount,
            UnmatchedExistingCount = unmatchedExisting
        };

        return (merged, stats);
    }

    /// <summary>
    /// Copies result fields from an existing row onto a pattern line; pattern structure and defaults win.
    /// </summary>
    public static ASTRowModel MergeResultOntoPatternLine(ASTRowModel patternLine, ASTRowModel existingRow)
    {
        if (patternLine == null)
        {
            return existingRow;
        }

        if (existingRow == null)
        {
            return patternLine;
        }

        var merged = patternLine;

        if (existingRow.ZoneDiameter.HasValue)
        {
            merged.ZoneDiameter = existingRow.ZoneDiameter;
        }

        if (existingRow.Mic.HasValue)
        {
            merged.Mic = existingRow.Mic;
        }

        if (!string.IsNullOrEmpty(existingRow.Operator))
        {
            merged.Operator = existingRow.Operator;
        }

        if (existingRow.TestResult != 0)
        {
            merged.TestResult = existingRow.TestResult;
        }

        if (existingRow.AppliedBreakpointId != 0)
        {
            merged.AppliedBreakpointId = existingRow.AppliedBreakpointId;
        }

        if (!string.IsNullOrEmpty(existingRow.IncludeOnReport))
        {
            merged.IncludeOnReport = existingRow.IncludeOnReport;
        }

        merged.EmbeddedASTRows = MergeEmbeddedRows(
            patternLine.EmbeddedASTRows,
            existingRow.EmbeddedASTRows);

        return merged;
    }

    private static List<ASTRowModel> MergeEmbeddedRows(
        List<ASTRowModel> patternEmbeds,
        List<ASTRowModel> existingEmbeds)
    {
        if (patternEmbeds == null || patternEmbeds.Count == 0)
        {
            return existingEmbeds ?? patternEmbeds;
        }

        if (existingEmbeds == null || existingEmbeds.Count == 0)
        {
            return patternEmbeds;
        }

        var existingBySpecialId = existingEmbeds
            .Where(e => e != null && e.SpecialConsiderationId != 0 && e.SpecialConsiderationId != AntibioticListForReportExtensions.NoSpecialConsiderationSentinelId)
            .GroupBy(e => e.SpecialConsiderationId)
            .ToDictionary(g => g.Key, g => g.First());

        return patternEmbeds.Select(patternEmbed =>
        {
            if (patternEmbed == null)
            {
                return patternEmbed;
            }

            if (!existingBySpecialId.TryGetValue(patternEmbed.SpecialConsiderationId, out var existingEmbed))
            {
                return patternEmbed;
            }

            var mergedEmbed = patternEmbed;

            if (existingEmbed.TestResult != 0)
            {
                mergedEmbed.TestResult = existingEmbed.TestResult;
            }

            if (existingEmbed.ZoneDiameter.HasValue)
            {
                mergedEmbed.ZoneDiameter = existingEmbed.ZoneDiameter;
            }

            if (existingEmbed.Mic.HasValue)
            {
                mergedEmbed.Mic = existingEmbed.Mic;
            }

            if (!string.IsNullOrEmpty(existingEmbed.Operator))
            {
                mergedEmbed.Operator = existingEmbed.Operator;
            }

            if (existingEmbed.AppliedBreakpointId != 0)
            {
                mergedEmbed.AppliedBreakpointId = existingEmbed.AppliedBreakpointId;
            }

            if (!string.IsNullOrEmpty(existingEmbed.IncludeOnReport))
            {
                mergedEmbed.IncludeOnReport = existingEmbed.IncludeOnReport;
            }

            return mergedEmbed;
        }).ToList();
    }
}
