using System.Collections.Generic;

namespace arc.common.Models.AST;

/// <summary>
/// An organism expert rule that has no actions (a guidance rule). On the AST screen it is shown in the inline
/// colour-coded hover next to the AST lines that triggered it; only guidance rules with no triggering AST line
/// fall back to the top alert strip.
/// </summary>
public class ExpertRuleCommentAlertModel
{
    public int RuleId { get; set; }

    public string RuleName { get; set; }

    /// <summary>
    /// Expert rule description (same as <c>RuleText</c> on the coding rule); displayed as the alert/hover body.
    /// </summary>
    public string RuleText { get; set; }

    /// <summary>
    /// AST lines (by id) that triggered this guidance rule. Empty when the rule has no triggering AST line, in which
    /// case the rule is shown in the top alert strip as a fallback.
    /// </summary>
    public List<ExpertRuleTriggerModel> Triggers { get; set; } = [];
}
