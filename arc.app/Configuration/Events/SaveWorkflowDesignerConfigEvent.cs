using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// Persists the workflow document sent by the workflow designer.
    /// </summary>
    /// <remarks>
    /// Only the workflow itself is written. The supporting state, event and list lookups the designer
    /// was given are reference data and are deliberately not part of the payload, so a designer save
    /// cannot alter listitem or the event configuration.
    /// </remarks>
    internal class SaveWorkflowDesignerConfigEvent : IRun, IProvideSaveResult<WorkflowDesignerSaveResultModel>
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveWorkflowDesignerConfigEvent"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve the repository and log writer.</param>
        public SaveWorkflowDesignerConfigEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the outcome of the most recent save, so the controller can return it to the designer.
        /// </summary>
        public WorkflowDesignerSaveResultModel SaveResult { get; private set; }

        /// <summary>
        /// Saves the workflow document.
        /// </summary>
        /// <param name="dataToSave">The JSON payload holding the workflow being saved.</param>
        /// <param name="id">The workflow name, used only for logging.</param>
        /// <param name="command">The event model describing the request.</param>
        /// <param name="eventData">Optional event configuration data, unused here.</param>
        /// <returns>The number of steps stored, or zero when nothing was written.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var logWriter = _serviceProvider.GetService<ILogWriter>();
            var configRepository = _serviceProvider.GetService<IConfigRepository>();
            var config = JsonConvert.DeserializeObject<SaveWorkflowDesignerConfigModel>(dataToSave);

            if (config?.Workflow == null)
            {
                logWriter?.LogError(
                    $"Workflow designer payload for '{id}' bound to no workflow document. Nothing was saved.",
                    nameof(SaveWorkflowDesignerConfigEvent), nameof(RunAsync));
                SaveResult = new WorkflowDesignerSaveResultModel();
                return 0;
            }

            if (config.ConfigId <= 0)
            {
                logWriter?.LogError(
                    $"Workflow designer payload for '{config.ConfigName}' arrived without a configs identity, so the save has to fall back to matching by name. This is the condition that used to let a workflow edit land on the wrong record.",
                    nameof(SaveWorkflowDesignerConfigEvent), nameof(RunAsync));
            }

            logWriter?.LogInfo(
                $"Workflow designer payload for '{config.ConfigName}' (id {config.ConfigId}) bound to a document with {CountSteps(config)} step(s).",
                nameof(SaveWorkflowDesignerConfigEvent), nameof(RunAsync));

            SaveResult = await configRepository.SaveWorkflowDesignerConfigAsync(config);

            return SaveResult?.StepCount ?? 0;
        }

        /// <summary>
        /// Counts the steps in the bound document so a payload that lost its steps in binding is
        /// visible in the log before anything is written.
        /// </summary>
        /// <param name="config">The bound configuration model.</param>
        /// <returns>The number of steps in the document.</returns>
        private static int CountSteps(SaveWorkflowDesignerConfigModel config)
        {
            return (config.Workflow.GetValue("Steps", StringComparison.OrdinalIgnoreCase) as Newtonsoft.Json.Linq.JArray)?.Count ?? 0;
        }
    }
}
