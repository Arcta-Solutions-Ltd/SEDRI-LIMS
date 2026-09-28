using System.Collections.Generic;

namespace arc.common.Models.AST;

/// <summary>
/// Expert rule section payload for the AST craft: grouped rules, a flat list of action rows for display,
/// and informational comment-only rules (no actions) for top-of-page alerts.
/// </summary>
public class ExpertRulePageDisplayResult
{
    public List<ExpertRuleGroupModel> ExpertRuleGroups { get; set; } = [];

    public List<ASTRowModel> ExpertRuleResults { get; set; } = [];

    /// <summary>
    /// Rules with no action grid: not shown under Expert Rules; the portal displays <see cref="ExpertRuleCommentAlertModel.RuleText"/> in the top alert list.
    /// </summary>
    public List<ExpertRuleCommentAlertModel> ExpertRuleCommentAlerts { get; set; } = [];
}
