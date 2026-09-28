using System.Collections.Generic;
using arc.common.ExtensionMethods;
using arc.common.Models.AST;
using Microsoft.Extensions.Logging;

namespace arc.app.AST;

/// <summary>
/// Validates manual susceptibility override audit data on AST save.
/// </summary>
public static class AstSusceptibilityOverrideValidator
{
    /// <summary>
    /// When lab audit is enabled, each manually set line must have a canned or free-text reason.
    /// Returns a language tag when validation fails, otherwise null.
    /// </summary>
    public static string ValidateSavePayload(
        IEnumerable<ASTModel> astResults,
        bool recordSusceptibilityChangeAudit,
        ILogger logger,
        int cultureId)
    {
        if (!recordSusceptibilityChangeAudit || astResults == null)
        {
            return null;
        }

        foreach (var row in astResults)
        {
            if (row?.ExpertRuleLine == true)
            {
                continue;
            }

            if (row?.SusceptibilityOverride?.ClearOverride == true)
            {
                continue;
            }

            if (row?.SusceptibilityOverride?.IsManuallySet != true)
            {
                continue;
            }

            if (!row.SusceptibilityOverride.HasRequiredOverrideReason())
            {
                logger.LogWarning(
                    "AstSusceptibilityOverrideValidator: Missing reason. CultureId={CultureId}, TestMethodId={TestMethodId}, AntibioticId={AntibioticId}",
                    cultureId,
                    row.TestMethodId,
                    row.AntibioticId);

                return "@AstSusOvrReq@";
            }

            if (row.SpecialRows != null)
            {
                foreach (var special in row.SpecialRows)
                {
                    if (special?.SusceptibilityOverride?.ClearOverride == true)
                    {
                        continue;
                    }

                    if (special?.SusceptibilityOverride?.IsManuallySet != true)
                    {
                        continue;
                    }

                    if (!special.SusceptibilityOverride.HasRequiredOverrideReason())
                    {
                        logger.LogWarning(
                            "AstSusceptibilityOverrideValidator: Missing embed reason. CultureId={CultureId}, SpecialTypeId={SpecialTypeId}",
                            cultureId,
                            special.SpecialTypeId);

                        return "@AstSusOvrReq@";
                    }
                }
            }
        }

        return null;
    }
}
