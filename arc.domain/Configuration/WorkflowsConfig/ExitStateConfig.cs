using System.Collections.Generic;

namespace arc.domain.Configuration.WorkflowsConfig
{
    /// <summary>
    /// Defines the possible state transitions that can follow a workflow step.
    /// Evaluates each conditional option in order and returns the first matching
    /// target state, falling back to <see cref="Default"/> when no option matches.
    /// </summary>
    public class ExitStateConfig
    {
        /// <summary>
        /// The workflow state to transition to when none of the conditional
        /// <see cref="Options"/> are satisfied. May be <c>null</c> or empty if no
        /// fallback state is required.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// The ordered list of conditional transition options. Options are evaluated
        /// in sequence and the first one whose conditions are fully satisfied is used.
        /// If the list is <c>null</c> or empty the result is always <see cref="Default"/>.
        /// </summary>
        public List<ExitStateConditionConfig> Options { get; set; }

        /// <summary>
        /// Determines the next workflow state by evaluating each option in <see cref="Options"/>
        /// in order. Returns the <see cref="ExitStateConditionConfig.NewState"/> of the first
        /// option whose conditions are satisfied, or <see cref="Default"/> if no option matches.
        /// </summary>
        /// <param name="currentState">
        /// The current workflow state of the record. Passed through to each option's
        /// condition evaluation so that state-based conditions can be tested.
        /// </param>
        /// <param name="message">
        /// The JSON string of the event message payload. Passed through to each option's
        /// condition evaluation so that field-based conditions can extract values from it.
        /// </param>
        /// <returns>
        /// The new state identifier to transition to, or <see cref="Default"/> when no
        /// conditional option is matched.
        /// </returns>
        public string GetNextState(string currentState, string message)
        {
            if (Options != null)
            {
                foreach (var option in Options)
                {
                    if (option.IsConditionMatched(currentState, message))
                    {
                        return option.NewState;
                    }
                }
            }
            return Default;
        }
    }
}
