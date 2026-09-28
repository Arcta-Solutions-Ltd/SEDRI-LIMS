using arc.common;
using arc.common.Models;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.WorkflowsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Configuration;

/// <summary>
/// Defines the contract for handling workflows, including operations such as retrieving workflow configurations,
/// determining the next state in a workflow based on a message, and obtaining action conditions for workflow events.
/// </summary>
public interface IWorkflowHandler
{
    /// <summary>
    /// Retrieves the workflow configuration for a specific workflow by its name.
    /// </summary>
    /// <param name="workflowName">The name of the workflow to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="WorkflowConfig"/>
    /// for the specified workflow.
    /// </returns>
    Task<WorkflowConfig> GetSingleWorkflowAsync(string workflowName);

    /// <summary>
    /// Asynchronously retrieves a list of workflow configurations.
    /// This method fetches all workflows available within the system.
    /// </summary>
    /// <returns>
    /// A task that resolves to a list of workflow configurations.
    /// </returns>
    Task<List<WorkflowConfig>> GetWorkflowListAsync();

    /// <summary>
    /// Processes a message in the context of a workflow event to determine the next state.
    /// </summary>
    /// <param name="message">The message to process.</param>
    /// <param name="model">The current event model which represents the state of the workflow.</param>
    /// <param name="eventData">The event configuration that provides context for state transitions.</param>
    /// <param name="token">The security token for the current session.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the updated <see cref="EventModel"/>
    /// representing the next state of the workflow after processing the message.
    /// </returns>
    Task<EventModel> GetNextStateAsync(string message, EventModel model, EventConfig eventData, TokenInfoModel token);

    /// <summary>
    /// Determines the action condition based on the provided event model and message.
    /// </summary>
    /// <param name="model">The current event model that holds workflow event details.</param>
    /// <param name="message">The message that may influence the action to be taken.</param>
    /// <returns>
    /// An instance of <see cref="ActionConditionConfig"/> representing the action or condition to take in response to the event.
    /// </returns>
    List<ActionConditionConfig> GetActions(EventModel model, string message);
}
