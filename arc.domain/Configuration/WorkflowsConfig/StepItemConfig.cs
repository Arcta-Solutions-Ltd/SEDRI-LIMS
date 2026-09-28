namespace arc.domain.Configuration.WorkflowsConfig
{
    /// <summary>
    /// Represents a single step (transition rule) within a workflow.
    /// A step is triggered by a named event when the record is in one of the specified
    /// entry states, and it defines how the workflow state should change via its
    /// exit-state configuration and any associated actions.
    /// </summary>
    public class StepItemConfig
    {
        /// <summary>
        /// A comma-separated list of workflow state identifiers from which this step
        /// can be triggered. The current record state must match one of these values
        /// (after trimming) for the step to be applied.
        /// </summary>
        public string EntryState { get; set; }

        /// <summary>
        /// The name of the event that triggers this step.
        /// Comparison against incoming events is case-insensitive.
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// An optional tag used to mark steps that require special handling outside
        /// the standard exit-state evaluation. Reserved for future extensibility.
        /// </summary>
        public string Special { get; set; }

        /// <summary>
        /// Defines the conditional state transitions available when this step fires.
        /// When <c>null</c> the workflow state remains unchanged after the step executes.
        /// </summary>
        public ExitStateConfig ExitState { get; set; }

        /// <summary>
        /// The set of side-effect actions (e.g. report publishing) that may be executed
        /// after the workflow transitions to a new state. Defaults to an empty
        /// <see cref="ActionConfig"/> so that callers can always enumerate <c>Actions.Options</c>
        /// without a null check.
        /// </summary>
        public ActionConfig Actions { get; set; } = new ActionConfig();

        /// <summary>
        /// Appends a new conditional action to <see cref="Actions"/>.
        /// </summary>
        /// <param name="action">
        /// The <see cref="ActionConditionConfig"/> to add. It will be evaluated when the
        /// workflow transitions to the state specified by <see cref="ActionConditionConfig.NewState"/>.
        /// </param>
        public void AddAction(ActionConditionConfig action)
        {
            Actions.Options.Add(action);
        }
    }
}
