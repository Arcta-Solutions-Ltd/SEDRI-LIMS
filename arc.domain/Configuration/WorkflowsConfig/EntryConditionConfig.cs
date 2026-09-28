using System.Collections.Generic;

namespace arc.domain.Configuration.WorkflowsConfig
{
    /// <summary>
    /// Represents the entry conditions for a workflow, defining which initial state a record
    /// should be placed into when it first enters the workflow. One or more event names are
    /// associated with this entry condition, and conditional options can override the default
    /// starting state based on the event message payload.
    /// </summary>
    public class EntryConditionConfig
    {
        /// <summary>
        /// A comma-separated list of event names that trigger this entry condition.
        /// Each entry in the list is compared case-insensitively against the current event name.
        /// </summary>
        public string Events { get; set; }

        /// <summary>
        /// The workflow state to assign when no conditional option in <see cref="Options"/> matches.
        /// This is the baseline starting state for the workflow when the triggering event fires.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// An optional ordered list of conditional state options evaluated against the event message
        /// payload. The first option whose conditions are satisfied determines the entry state.
        /// When <c>null</c> or empty, <see cref="Default"/> is always used.
        /// </summary>
        public List<ExitStateConditionConfig> Options { get; set; }

        /// <summary>
        /// Determines the initial workflow state for a record entering the workflow.
        /// Evaluates each option in <see cref="Options"/> in order using the supplied message,
        /// returning the <see cref="ExitStateConditionConfig.NewState"/> of the first matched option,
        /// or <see cref="Default"/> if no option is satisfied.
        /// </summary>
        /// <param name="message">
        /// The JSON string of the event message payload. Used to evaluate field-based conditions
        /// in each <see cref="ExitStateConditionConfig"/> option so that the appropriate entry
        /// state can be chosen based on the submitted data.
        /// </param>
        /// <returns>
        /// The initial workflow state identifier selected by the first matching conditional option,
        /// or <see cref="Default"/> when no option matches.
        /// </returns>
        public string GetEntryState(string message)
        {
            if (Options != null)
            {
                foreach (var option in Options)
                {
                    if (option.IsConditionMatched("", message))
                    {
                        return option.NewState;
                    }
                }
            }
            return Default;
        }
    }
}
