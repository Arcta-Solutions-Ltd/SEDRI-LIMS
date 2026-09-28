using arc.common.Models.AST;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Helpers for manual AST susceptibility override audit (id-based line keys only).
/// </summary>
public static class AstSusceptibilityOverrideExtensions
{
    /// <summary>
    /// Stable key for matching override rows to AST lines (ids only — never translated labels).
    /// </summary>
    public static string BuildSusceptibilityLineKey(
        int testMethodId,
        int antibioticId,
        int guidelinesId,
        int dosage,
        int specialConsiderationId)
    {
        if (testMethodId == 0 || antibioticId == 0 || guidelinesId == 0)
        {
            return string.Empty;
        }

        var normalizedDosage = testMethodId == AstRowModelExtensions.DiskTestMethodId ? dosage : 0;
        return $"{testMethodId}|{antibioticId}|{guidelinesId}|{normalizedDosage}|{specialConsiderationId}";
    }

    /// <summary>
    /// Builds the susceptibility line key for a portal AST row (parent line).
    /// </summary>
    public static string BuildSusceptibilityLineKey(this ASTRowModel row, int specialConsiderationId = 0)
    {
        if (row == null)
        {
            return string.Empty;
        }

        var testMethodId = row.TestMethod != 0
            ? row.TestMethod
            : row.ZoneDiameter.HasValue && row.ZoneDiameter.Value != 0
                ? AstRowModelExtensions.DiskTestMethodId
                : AstRowModelExtensions.MicTestMethodId;

        return BuildSusceptibilityLineKey(
            testMethodId,
            row.Antibiotic ?? 0,
            row.Guidelines,
            row.Dosage,
            specialConsiderationId);
    }

    /// <summary>
    /// True when the row has a manually set susceptibility that should be protected from auto-recalc.
    /// </summary>
    public static bool IsManuallySetSusceptibility(this ASTRowModel row)
    {
        return row?.SusceptibilityOverride?.IsManuallySet == true;
    }

    /// <summary>
    /// True when audit ON requires a reason and one is present (canned id or free text).
    /// </summary>
    public static bool HasRequiredOverrideReason(this AstSusceptibilityOverrideModel? model)
    {
        if (model == null)
        {
            return false;
        }

        return model.CannedCommentId > 0 || !string.IsNullOrWhiteSpace(model.FreeTextComment);
    }
}
