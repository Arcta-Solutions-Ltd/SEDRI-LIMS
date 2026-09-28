using arc.common.Models.AST;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Id-based matching helpers for manual AST disk/MIC lines against test pattern lines.
/// </summary>
public static class AstRowModelExtensions
{
    public const int DiskTestMethodId = 681;
    public const int MicTestMethodId = 680;

    /// <summary>
    /// Stable key for matching a manual AST line to a test pattern line (ids only — never translated labels).
    /// Disk lines include dosage; MIC lines ignore dosage.
    /// </summary>
    /// <param name="row">AST row from the portal or persistence layer.</param>
    /// <returns>Match key, or empty string when the row cannot be matched.</returns>
    public static string BuildLineMatchKey(this ASTRowModel row)
    {
        if (row == null || row.ExpertRuleLine)
        {
            return string.Empty;
        }

        var antibioticId = row.Antibiotic ?? 0;
        if (antibioticId == 0 || row.Guidelines == 0)
        {
            return string.Empty;
        }

        var testMethodId = row.TestMethod != 0 ? row.TestMethod : 0;
        if (testMethodId != DiskTestMethodId && testMethodId != MicTestMethodId)
        {
            return string.Empty;
        }

        var dosage = testMethodId == DiskTestMethodId ? row.Dosage : 0;
        return $"{testMethodId}|{antibioticId}|{row.Guidelines}|{dosage}";
    }

    /// <summary>
    /// True when two manual rows represent the same test pattern line (id-based key equality).
    /// </summary>
    public static bool IsSameTestPatternLine(this ASTRowModel a, ASTRowModel b)
    {
        if (a == null || b == null)
        {
            return false;
        }

        var keyA = a.BuildLineMatchKey();
        var keyB = b.BuildLineMatchKey();
        return keyA.Length > 0 && keyA == keyB;
    }

    /// <summary>
    /// True when the row is a mergeable manual disk/MIC line (not an expert-rule line).
    /// </summary>
    public static bool IsMergeableManualLine(this ASTRowModel row)
    {
        return row != null && !row.ExpertRuleLine && row.BuildLineMatchKey().Length > 0;
    }

    /// <summary>
    /// True when the row has an entered MIC (680) or zone diameter (681) suitable for expert-rule measurement-range checks.
    /// Treats null, 0, and -1 as no measurement — consistent with AST save sentinel and susceptibility lookup.
    /// </summary>
    /// <param name="row">AST row from the portal or persistence layer.</param>
    /// <returns><c>true</c> when a real measurement is present for range evaluation.</returns>
    public static bool HasEnteredMeasurementForExpertRuleRange(this ASTRowModel row)
    {
        return TryGetEnteredMeasurementForExpertRuleRange(row, out _);
    }

    /// <summary>
    /// Returns the numeric measurement for expert-rule range checks when the row has an entered value.
    /// Blank sentinels (null, 0, -1) are excluded so open-ended lower bounds (e.g. MIC ≤ 0.06) do not match empty fields.
    /// </summary>
    /// <param name="row">AST row from the portal or persistence layer.</param>
    /// <param name="measured">The measurement value when this method returns <c>true</c>.</param>
    /// <returns><c>true</c> when a real measurement is present for range evaluation.</returns>
    public static bool TryGetEnteredMeasurementForExpertRuleRange(this ASTRowModel row, out decimal measured)
    {
        measured = default;
        if (row == null)
        {
            return false;
        }

        if (row.TestMethod == MicTestMethodId)
        {
            if (!row.Mic.HasValue || IsBlankMicSentinel(row.Mic.Value))
            {
                return false;
            }

            measured = row.Mic.Value;
            return true;
        }

        if (row.TestMethod == DiskTestMethodId)
        {
            if (!row.ZoneDiameter.HasValue || IsBlankDiskSentinel(row.ZoneDiameter.Value))
            {
                return false;
            }

            measured = row.ZoneDiameter.Value;
            return true;
        }

        return false;
    }

    private static bool IsBlankMicSentinel(decimal value)
    {
        return value == 0m || value == -1m;
    }

    private static bool IsBlankDiskSentinel(int value)
    {
        return value == 0 || value == -1;
    }
}
