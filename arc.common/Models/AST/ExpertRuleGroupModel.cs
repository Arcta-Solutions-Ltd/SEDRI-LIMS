using System.Collections.Generic;

namespace arc.common.Models.AST;

/// <summary>
/// One display group per expert rule: identity, whether the rule is applied for this culture, and one row per rule action.
/// </summary>
public class ExpertRuleGroupModel
{
    public int RuleId { get; set; }
    public string RuleName { get; set; }
    public string RuleText { get; set; }
    /// <summary>
    /// When no AST is persisted for the culture, true for every valid rule; otherwise true only when this rule id was saved and is still in the calculated valid set.
    /// </summary>
    public bool ApplyRule { get; set; }
    public List<ASTRowModel> Actions { get; set; } = [];

    /// <summary>
    /// AST lines (by id) that triggered this rule. Empty for intrinsic rules or rules that fired only via isolate
    /// test conditions; the AST screen uses these to draw the inline trigger icon and colour-coded hover next to the matching lines.
    /// </summary>
    public List<ExpertRuleTriggerModel> Triggers { get; set; } = [];
}
