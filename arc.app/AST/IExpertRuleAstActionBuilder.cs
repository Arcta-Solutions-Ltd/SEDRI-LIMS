using arc.common.Models.AST;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpertRuleModel = arc.domain.Coding.ExpertRule;

namespace arc.app.AST;

/// <summary>
/// Builds AST expert-rule action rows from rule action grid entries, expanding antibiotic group actions
/// and filtering all rows by the laboratory's allowed antibiotic list.
/// </summary>
public interface IExpertRuleAstActionBuilder
{
    /// <summary>
    /// Expands group actions to every member antibiotic, then filters all action rows (single-antibiotic and
    /// expanded group lines) to those whose <c>antibiotic.id</c> appears in the laboratory allow list derived
    /// from <c>laboratory.antibioticgroupids</c>.
    /// </summary>
    /// <param name="cultureId">Culture id for logging.</param>
    /// <param name="cultureDetails">Culture context including laboratory id.</param>
    /// <param name="rule">Expert rule whose actions are being built.</param>
    /// <param name="testMethod">Resolved AST test method (681 Disk / 680 MIC) for the action rows.</param>
    /// <param name="allowedLaboratoryAntibioticIds">
    /// Union of all <c>antibiotic.id</c> values in the culture laboratory's configured antibiotic groups.
    /// <c>null</c> when the laboratory selected Master (all groups); empty when no groups or resolution failed.
    /// </param>
    /// <returns>AST rows for the Expert Rules section, or an empty list when filtered out.</returns>
    Task<List<ASTRowModel>> BuildActionRowsAsync(
        string cultureId,
        ASTCultureModel cultureDetails,
        ExpertRuleModel rule,
        int testMethod,
        IReadOnlySet<int>? allowedLaboratoryAntibioticIds);
}
