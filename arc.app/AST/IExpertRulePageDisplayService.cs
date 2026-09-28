using System.Collections.Generic;
using System.Threading.Tasks;
using arc.common.Models.AST;

namespace arc.app.AST;

/// <summary>
/// Builds the AST expert-rules section (groups and flat result rows) from organism-scoped rules and current disk/MIC rows.
/// </summary>
public interface IExpertRulePageDisplayService
{
    /// <summary>
    /// Evaluates which expert rules apply for display using flattened disk/MIC rows (including special-consideration embeds),
    /// culture test field lines, and specimen scope. Each group with actions gets <see cref="ExpertRuleGroupModel.ApplyRule"/>
    /// <c>true</c> by default (first-time display); the portal merges prior client state on refresh so print-only-only groups
    /// and explicit Apply off are preserved.
    /// Rules with no actions are returned in <see cref="ExpertRulePageDisplayResult.ExpertRuleCommentAlerts"/> for top-of-page alerts only.
    /// </summary>
    /// <param name="cultureId">Culture id string (for loading isolate tests).</param>
    /// <param name="cultureDetails">Organism, specimen, laboratory context from the culture.</param>
    /// <param name="diskResults">Disk (zone) AST rows as shown in the UI.</param>
    /// <param name="micResults">MIC AST rows as shown in the UI.</param>
    /// <param name="persistedAstRowCount">Number of AST rows persisted for this culture (0 when only test-pattern draft); reserved for callers.</param>
    /// <param name="savedExpertRuleIdsFromPersistence">Distinct <c>ExpertRuleId</c> values from persisted AST rows; reserved for callers.</param>
    Task<ExpertRulePageDisplayResult> BuildDisplayAsync(
        string cultureId,
        ASTCultureModel cultureDetails,
        IEnumerable<ASTRowModel> diskResults,
        IEnumerable<ASTRowModel> micResults,
        int persistedAstRowCount,
        IReadOnlyCollection<int> savedExpertRuleIdsFromPersistence);
}
