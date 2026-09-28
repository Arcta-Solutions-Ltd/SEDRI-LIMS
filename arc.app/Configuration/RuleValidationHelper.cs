using arc.common.Models.Config;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Configuration
{
    /// <summary>
    /// Validates and filters rule configurations. A rule is valid only when Effect, Field, and Rule
    /// are non-empty, and Value is non-empty when Rule is "=" or "!=".
    /// Incomplete rules are excluded from persistence.
    /// </summary>
    internal static class RuleValidationHelper
    {
        /// <summary>
        /// Returns true if the rule has all required values filled. Effect, Field, and Rule must be
        /// non-empty; Value must be non-empty when Rule is "=" or "!=".
        /// </summary>
        /// <param name="rule">The rule to validate.</param>
        /// <returns>True if the rule is complete and can be saved.</returns>
        internal static bool IsRuleValid(FormGroupRuleModel rule)
        {
            if (rule == null) return false;
            if (string.IsNullOrWhiteSpace(rule.Effect)) return false;
            if (string.IsNullOrWhiteSpace(rule.Field)) return false;
            if (string.IsNullOrWhiteSpace(rule.Rule)) return false;

            var ruleLower = rule.Rule.Trim().ToLowerInvariant();
            if (ruleLower == "=" || ruleLower == "!=")
            {
                return !string.IsNullOrWhiteSpace(rule.Value);
            }

            return true;
        }

        /// <summary>
        /// Filters a list of rules to only include valid ones. Invalid rules (missing required
        /// Effect, Field, Rule, or Value when needed) are excluded.
        /// </summary>
        /// <param name="rules">The rules to filter. May be null.</param>
        /// <returns>List of valid rules only.</returns>
        internal static List<FormGroupRuleModel> FilterValidRules(IEnumerable<FormGroupRuleModel> rules)
        {
            if (rules == null) return new List<FormGroupRuleModel>();
            return rules.Where(IsRuleValid).ToList();
        }
    }
}
