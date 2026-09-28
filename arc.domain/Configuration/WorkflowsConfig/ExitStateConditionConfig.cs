using System.Collections.Generic;

namespace arc.domain.Configuration.WorkflowsConfig
{
    /// <summary>
    /// Represents one option within an exit-state or action options list.
    /// Holds the target new state, the logical combination type for its conditions,
    /// and the list of individual conditions that must be evaluated.
    /// </summary>
    public class ExitStateConditionConfig
    {
        /// <summary>
        /// The ordered list of <see cref="WorkflowConditionConfig"/> conditions that are
        /// evaluated together according to <see cref="ConditionType"/>.
        /// </summary>
        public List<WorkflowConditionConfig> Conditions { get; set; }

        /// <summary>
        /// The logical operator used to combine multiple conditions.
        /// Supported values are <c>"and"</c> (all conditions must match) and
        /// <c>"or"</c> (any condition matching is sufficient). Comparison is case-insensitive.
        /// Defaults to treating all conditions as required when the value is absent.
        /// </summary>
        public string ConditionType { get; set; }

        /// <summary>
        /// The workflow state identifier to transition to when all conditions in this option are satisfied.
        /// </summary>
        public string NewState { get; set; }

        /// <summary>
        /// Evaluates whether all (or any, depending on <see cref="ConditionType"/>) of the
        /// contained conditions are satisfied.
        /// Short-circuits on the first deciding result: an unmatched condition causes an immediate
        /// <c>false</c> return for <c>"and"</c>, and a matched condition causes an immediate
        /// <c>true</c> return for <c>"or"</c>.
        /// </summary>
        /// <param name="currentState">
        /// The current workflow state of the record, passed through to each
        /// <see cref="WorkflowConditionConfig.IsConditionMatched"/> evaluation.
        /// </param>
        /// <param name="message">
        /// The JSON string of the event message payload, passed through to each
        /// <see cref="WorkflowConditionConfig.IsConditionMatched"/> evaluation so that
        /// field-based conditions can extract values from it.
        /// </param>
        /// <returns>
        /// <c>true</c> if the combined set of conditions is satisfied according to
        /// <see cref="ConditionType"/>; <c>false</c> otherwise.
        /// </returns>
        public bool IsConditionMatched(string currentState, string message)
        {
            foreach (var condition in Conditions)
            {
                var matched = condition.IsConditionMatched(currentState, message);

                if (matched && (ConditionType ?? "").ToLower() == "or")
                {
                    return true;
                }

                if (!matched && (ConditionType ?? "").ToLower() == "and")
                {
                    return false;
                }
            }
            return true;
        }
    }
}
