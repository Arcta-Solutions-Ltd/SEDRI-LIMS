using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.WorkflowsConfig;

/// <summary>
/// Represents the configuration model for a workflow.
/// Defines properties and methods to manage workflow states, steps, and transitions.
/// </summary>
public class WorkflowConfig
{
    /// <summary>
    /// Unique identifier for the workflow.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the workflow.
    /// Provides a human-readable identifier for the workflow process.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the workflow.
    /// Helps explain the purpose or function of this workflow.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The database table associated with this workflow.
    /// Used to store and manage workflow-related data.
    /// </summary>
    public string Table { get; set; }

    /// <summary>
    /// The field within the table that the workflow operates on.
    /// Defines the specific column affected by workflow actions.
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// The starting state of the workflow.
    /// Defines the initial position in the workflow process.
    /// </summary>
    public string StartState { get; set; }

    public bool IncludeCulture { get; set; } = true;
    public bool IncludeInstrument { get; set; } = true;

    /// <summary>
    /// List of steps within the workflow.
    /// Each step represents an actionable transition in the process.
    /// </summary>
    public List<StepItemConfig> Steps { get; set; }

    /// <summary>
    /// A list of possible states within this workflow.
    /// Helps track and define workflow progress.
    /// </summary>
    public string StatesList { get; set; }

    /// <summary>
    /// List of entry conditions that determine initial workflow states.
    /// These conditions help regulate workflow transitions.
    /// </summary>
    public List<EntryConditionConfig> EntryConditions { get; set; }

    /// <summary>
    /// List of default test configurations tied to the workflow.
    /// Ensures workflow functionality aligns with predefined checks.
    /// </summary>
    public List<DefaultTestConfig> DefaultTests { get; set; }

    /// <summary>
    /// Determines the next state based on the current state, an event trigger, and a message.
    /// </summary>
    /// <param name="currentState">The current state of the workflow.</param>
    /// <param name="eventName">The event that may trigger a state transition.</param>
    /// <param name="message">Optional message influencing the state transition.</param>
    /// <returns>The next determined workflow state.</returns>
    public string GetNextState(string currentState, string eventName, string message)
    {
        var returnState = "";

        if (string.IsNullOrEmpty(currentState))
        {
            foreach (var condition in EntryConditions)
            {
                if (returnState == "")
                {
                    var entryEvents = condition.Events.Split(",");
                    foreach (var entryEvent in entryEvents)
                    {
                        if (entryEvent.ToLower() == eventName.ToLower())
                        {
                            returnState = condition.GetEntryState(message);
                        }
                    }
                }
            }
        }
        else
        {
            var steps = Steps.Where(s => s.Event != null && s.Event.ToLower() == eventName.ToLower());
            if (steps.Count() > 0)
            {
                var step = steps.First();
                var entryStates = step.EntryState.Split(",");
                foreach (var entryState in entryStates)
                {
                    if (entryState.Trim() == currentState)
                    {
                        returnState = step.ExitState?.GetNextState(currentState, message) ?? currentState;
                    }
                }
            }
            else
            {
                returnState = currentState;
            }
        }

        return returnState;
    }

    /// <summary>
    /// Adds a new step to the workflow.
    /// </summary>
    /// <param name="newStep">The step to be added.</param>
    public void AddStep(StepItemConfig newStep)
    {
        Steps.Add(newStep);
    }

    /// <summary>
    /// Deletes a step from the workflow based on the given event name.
    /// </summary>
    /// <param name="eventName">The event name identifying the step to be deleted.</param>
    public void DeleteStep(string eventName)
    {
        var stepToDelete = GetStep(eventName);
        Steps.Remove(stepToDelete);
    }

    /// <summary>
    /// Retrieves a step associated with the given event name.
    /// </summary>
    /// <param name="eventName">The event name corresponding to the step.</param>
    /// <returns>The step configuration matching the event name.</returns>
    public StepItemConfig GetStep(string eventName)
    {
        return Steps.First(x => x.Event.ToLower() == eventName.ToLower());
    }

    /// <summary>
    /// Checks if a given event exists within the workflow.
    /// </summary>
    /// <param name="eventName">The event to check.</param>
    /// <returns>True if the event is found in entry conditions or steps, otherwise false.</returns>
    public bool CheckIfEventInWorkflow(string eventName)
    {
        return EntryConditions.Any(c => c.Events.ToLower() == eventName.ToLower()) || Steps.Any(s => s.Event.ToLower() == eventName.ToLower());
    }
}
