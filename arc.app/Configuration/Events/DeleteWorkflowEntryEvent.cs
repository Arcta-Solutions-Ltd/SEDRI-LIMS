using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.Config.Workflows;
using arc.app.SystemConfig;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// Removes a step from a workflow.
    /// </summary>
    /// <remarks>
    /// The workflow written to is the one the request identifies, resolved by <see cref="WorkflowEntryTarget"/>.
    /// The step itself is identified by its event name, which is the row id the workflow step list uses.
    /// </remarks>
    internal class DeleteWorkflowEntryEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteWorkflowEntryEvent"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve repositories and the log writer.</param>
        public DeleteWorkflowEntryEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Deletes the step and writes the workflow back.
        /// </summary>
        /// <param name="dataToSave">The JSON payload of the delete workflow entry form.</param>
        /// <param name="id">The event name of the step to remove.</param>
        /// <param name="command">The event model describing the request. Its Id identifies the workflow when the form supplies one.</param>
        /// <param name="eventData">Optional event configuration data, unused here.</param>
        /// <returns>Zero; the caller treats this as a configuration event with no new record id.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var workflowAdapter = _serviceProvider.GetService<IWorkflowAdapter>();
            var configRepository = _serviceProvider.GetService<IConfigRepository>();
            var logWriter = _serviceProvider.GetService<ILogWriter>();

            // The delete form's id is the step's event name, so the workflow can only come from the
            // event model. Anything else would resolve to whichever workflow shares that step name.
            var target = await WorkflowEntryTarget.ResolveAsync(
                command?.Id, configRepository, workflowAdapter, logWriter, nameof(DeleteWorkflowEntryEvent));

            target.Workflow.DeleteStep(id);

            logWriter?.LogInfo(
                $"Removed step '{id}' from workflow '{target.ConfigName}' (id {target.ConfigId}); the workflow now has {target.Workflow.Steps?.Count ?? 0} step(s).",
                nameof(DeleteWorkflowEntryEvent), nameof(RunAsync));

            await target.SaveAsync(configRepository);

            return 0;
        }
    }
}
