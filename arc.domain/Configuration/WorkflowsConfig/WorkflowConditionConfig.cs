using arc.common.Utils;

namespace arc.domain.Configuration.WorkflowsConfig
{
    /// <summary>
    /// Represents a single condition within a workflow transition option.
    /// A condition can either test the current workflow state or evaluate a field value
    /// extracted from the event message payload.
    /// </summary>
    public class WorkflowConditionConfig
    {
        /// <summary>
        /// The workflow state that must be current for this condition to match.
        /// When set, the field-based check (<see cref="Field"/> / <see cref="Value"/>) is ignored
        /// and only the current state is compared. When null or empty, the field-based check is used instead.
        /// </summary>
        public string CurrentState { get; set; }

        /// <summary>
        /// The name of the JSON field in the event message payload to evaluate.
        /// Used only when <see cref="CurrentState"/> is null or empty.
        /// Field name matching against the message is case-insensitive.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// The expected value of the field identified by <see cref="Field"/>.
        /// The comparison is case-insensitive, so "Active" and "active" are treated as equal.
        /// Used only when <see cref="CurrentState"/> is null or empty.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Determines whether this condition is satisfied given the current workflow state and event message.
        /// When <see cref="CurrentState"/> is set the condition tests the current state only.
        /// When <see cref="CurrentState"/> is absent the condition extracts the value of <see cref="Field"/>
        /// from <paramref name="message"/> and compares it to <see cref="Value"/>.
        /// Both comparisons are case-insensitive so that differences in casing between the
        /// workflow configuration and the submitted form data (e.g. "growthid" vs "GrowthId")
        /// do not cause false negatives.
        /// </summary>
        /// <param name="currentState">The current workflow state of the record being processed.</param>
        /// <param name="message">
        /// The JSON string of the event message payload submitted by the client. Used to extract
        /// the value of <see cref="Field"/> when <see cref="CurrentState"/> is not set.
        /// </param>
        /// <returns>
        /// <c>true</c> if the condition is satisfied; <c>false</c> otherwise.
        /// </returns>
        public bool IsConditionMatched(string currentState, string message)
        {
            if (string.IsNullOrEmpty(CurrentState))
            {
                var jsonElementRemover = new JsonElementRemover();
                var jsonReplacer = new JsonReplacer(jsonElementRemover);
                var fieldValue = jsonReplacer.GetValueInJsonString(message, Field);
                return string.Equals(fieldValue, Value, System.StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return string.Equals(currentState, CurrentState, System.StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
