using System.Collections.Generic;
using arc.common.ExtensionMethods;
using arc.common.Models.AST;
using arc.data.model.AST;

namespace arc.app.AST;

/// <summary>
/// Builds astsusceptibilityoverride rows from flattened AST save payload.
/// </summary>
public static class AstSusceptibilityOverrideCollector
{
    /// <summary>
    /// Collects override rows to upsert from the AST save payload (parent and embed lines).
    /// </summary>
    public static List<AstSusceptibilityOverrideDataModel> CollectFromSavePayload(
        int cultureId,
        IEnumerable<ASTModel> results,
        string username)
    {
        var list = new List<AstSusceptibilityOverrideDataModel>();
        if (results == null)
        {
            return list;
        }

        foreach (var row in results)
        {
            if (row?.ExpertRuleLine == true)
            {
                continue;
            }

            if (!int.TryParse(row.TestMethod, out var testMethodId))
            {
                testMethodId = row.TestMethodId;
            }

            if (!int.TryParse(row.Antibiotic, out var antibioticId))
            {
                antibioticId = row.AntibioticId;
            }

            if (!int.TryParse(row.Guidelines, out var guidelinesId))
            {
                guidelinesId = row.GuidelinesId;
            }

            if (row.SusceptibilityOverride?.ClearOverride != true &&
                row.SusceptibilityOverride?.IsManuallySet == true)
            {
                var parent = AstSusceptibilityOverrideMerger.ToDataModel(
                    cultureId,
                    testMethodId,
                    antibioticId,
                    guidelinesId,
                    row.Dosage,
                    0,
                    row.SusceptibilityOverride,
                    username);

                if (parent != null)
                {
                    list.Add(parent);
                }
            }

            if (row.SpecialRows == null)
            {
                continue;
            }

            foreach (var special in row.SpecialRows)
            {
                if (special?.SusceptibilityOverride?.ClearOverride == true ||
                    special?.SusceptibilityOverride?.IsManuallySet != true)
                {
                    continue;
                }

                var embed = AstSusceptibilityOverrideMerger.ToDataModel(
                    cultureId,
                    testMethodId,
                    antibioticId,
                    guidelinesId,
                    row.Dosage,
                    special.SpecialTypeId,
                    special.SusceptibilityOverride,
                    username);

                if (embed != null)
                {
                    list.Add(embed);
                }
            }
        }

        return list;
    }
}
