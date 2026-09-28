using System.Collections.Generic;

namespace arc.common.Models.Config
{
    /// <summary>
    /// Payload for the editpagerules configuration event.
    /// Id is "formName|pageName" identifying the page whose rules are being configured.
    /// </summary>
    public class PageRulesModel : AddPageModel
    {
        /// <summary>
        /// Visibility and state setting configuration captured for the page.
        /// </summary>
        public PageRuleDetailModel PageRules { get; set; }
    }

    /// <summary>
    /// One rule row in "This page sets the state". Effect is the state token applied when the field
    /// comparison passes. Rows sharing the same Effect are ANDed at runtime.
    /// </summary>
    public class PageRuleStateRuleModel
    {
        /// <summary>
        /// State token this row contributes to (maps to RuleConfig.Effect).
        /// </summary>
        public string Effect { get; set; }

        /// <summary>
        /// Identifier of the field whose value is compared.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Comparison used against the field. One of "=", "!=", "isempty" or "isnotempty".
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// Value the field is compared against. Always a list item id rather than display text when
        /// the field is list backed, so rules survive translation.
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// Visibility and state setting configuration for a single page of a form.
    /// </summary>
    public class PageRuleDetailModel
    {
        /// <summary>
        /// State that must be active for the page to be visible. Empty means the page is always visible.
        /// </summary>
        public string VisibleWhenState { get; set; }

        /// <summary>
        /// All state-setting rules for this page.
        /// </summary>
        public List<PageRuleStateRuleModel> StateRules { get; set; }

        /// <summary>
        /// Legacy state name from the first rule. Populated on read for backward compatibility.
        /// </summary>
        public string SetsState { get; set; }

        /// <summary>
        /// Legacy condition field from the first rule. Populated on read for backward compatibility.
        /// </summary>
        public string ConditionField { get; set; }

        /// <summary>
        /// Legacy comparison from the first rule. Populated on read for backward compatibility.
        /// </summary>
        public string ConditionRule { get; set; }

        /// <summary>
        /// Legacy value from the first rule. Populated on read for backward compatibility.
        /// </summary>
        public string ConditionValue { get; set; }
    }
}
