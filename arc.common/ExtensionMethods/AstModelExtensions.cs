using arc.common.Models.AST;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Id-based helpers for flattened AST save payload rows (<see cref="ASTModel"/>).
/// </summary>
public static class AstModelExtensions
{
    public const int DiskTestMethodId = 681;
    public const int MicTestMethodId = 680;

    /// <summary>
    /// True when <see cref="ASTModel.Susceptibility"/> or <see cref="ASTModel.SusceptibilityId"/> resolves to a non-zero list-item id.
    /// </summary>
    /// <param name="row">Flattened AST result row from the save payload.</param>
    public static bool HasResolvedSusceptibility(this ASTModel row)
    {
        return row.TryGetSusceptibilityId(out _);
    }

    /// <summary>
    /// Parses susceptibility from id property or string field (ids only — never translated labels).
    /// </summary>
    /// <param name="row">Flattened AST result row from the save payload.</param>
    /// <param name="susceptibilityId">Resolved susceptibility list-item id when true.</param>
    public static bool TryGetSusceptibilityId(this ASTModel row, out int susceptibilityId)
    {
        susceptibilityId = 0;
        if (row == null)
        {
            return false;
        }

        if (row.SusceptibilityId > 0)
        {
            susceptibilityId = row.SusceptibilityId;
            return true;
        }

        if (!string.IsNullOrWhiteSpace(row.Susceptibility) &&
            int.TryParse(row.Susceptibility, out var parsed) &&
            parsed > 0)
        {
            susceptibilityId = parsed;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Parses antibiotic id from id property or string field.
    /// </summary>
    /// <param name="row">Flattened AST result row from the save payload.</param>
    /// <param name="antibioticId">Resolved antibiotic id when true.</param>
    public static bool TryGetAntibioticId(this ASTModel row, out int antibioticId)
    {
        antibioticId = 0;
        if (row == null)
        {
            return false;
        }

        if (row.AntibioticId > 0)
        {
            antibioticId = row.AntibioticId;
            return true;
        }

        if (!string.IsNullOrWhiteSpace(row.Antibiotic) &&
            int.TryParse(row.Antibiotic, out var parsed) &&
            parsed > 0)
        {
            antibioticId = parsed;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Resolves Disk (681) or MIC (680) test method id from the save payload row.
    /// </summary>
    /// <param name="row">Flattened AST result row from the save payload.</param>
    /// <param name="testMethodId">681 or 680 when true.</param>
    public static bool TryGetTestMethodId(this ASTModel row, out int testMethodId)
    {
        testMethodId = 0;
        if (row == null)
        {
            return false;
        }

        if (row.TestMethodId == DiskTestMethodId || row.TestMethodId == MicTestMethodId)
        {
            testMethodId = row.TestMethodId;
            return true;
        }

        if (!string.IsNullOrWhiteSpace(row.TestMethod) &&
            int.TryParse(row.TestMethod, out var parsed) &&
            (parsed == DiskTestMethodId || parsed == MicTestMethodId))
        {
            testMethodId = parsed;
            return true;
        }

        return false;
    }

    /// <summary>
    /// True when the row has a resolved susceptibility id and may participate in duplicate-antibiotic validation.
    /// </summary>
    public static bool ParticipatesInDuplicateCheck(this ASTModel row)
    {
        return row != null && row.HasResolvedSusceptibility() && row.TryGetAntibioticId(out _);
    }
}
