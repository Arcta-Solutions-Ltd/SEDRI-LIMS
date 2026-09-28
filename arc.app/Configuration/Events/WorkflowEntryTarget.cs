using arc.app.Common;
using arc.app.Config.Workflows;
using arc.app.SystemConfig;
using arc.common.Models.Config;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.WorkflowsConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// The workflow a step edit is going to be written to, resolved from the request rather than
    /// assumed.
    /// </summary>
    /// <remarks>
    /// Step edits used to write to a record named <c>SpecimenDefault</c> whatever workflow the user
    /// had open, and the update matched on configuration name alone with no ConfigTypeId filter. This
    /// resolves the target from the configs identity the request carries, scoped to the workflow
    /// ConfigType, and only falls back to the default workflow when there is genuinely no id to work
    /// from, which it logs as an error.
    /// </remarks>
    internal class WorkflowEntryTarget
    {
        /// <summary>
        /// The configuration name of the workflow that will be written.
        /// </summary>
        internal const string DefaultWorkflowName = "SpecimenDefault";

        /// <summary>
        /// The ConfigTypeId workflow documents are stored under.
        /// </summary>
        internal const int WorkflowConfigTypeId = 18;

        /// <summary>
        /// Gets the configs identity of the workflow, or zero when it had to be resolved by name.
        /// </summary>
        internal int ConfigId { get; private init; }

        /// <summary>
        /// Gets the configuration name of the workflow being written.
        /// </summary>
        internal string ConfigName { get; private init; }

        /// <summary>
        /// Gets the workflow itself.
        /// </summary>
        internal WorkflowConfig Workflow { get; private init; }

        /// <summary>
        /// Works out which workflow a step edit belongs to and loads it.
        /// </summary>
        /// <param name="candidateId">The identifier the request supplied, which may be a configs identity or something else entirely.</param>
        /// <param name="configRepository">Repository used to read the configs row.</param>
        /// <param name="workflowAdapter">Adapter used for the fall back load by name.</param>
        /// <param name="logWriter">The log writer.</param>
        /// <param name="caller">The event resolving the target, for logging.</param>
        /// <returns>The workflow to edit, never null.</returns>
        internal static async Task<WorkflowEntryTarget> ResolveAsync(
            string candidateId,
            IConfigRepository configRepository,
            IWorkflowAdapter workflowAdapter,
            ILogWriter logWriter,
            string caller)
        {
            if (int.TryParse(candidateId, out var configId) && configId > 0)
            {
                var filters = new QueryFilterConfig();
                filters.AddString("id", configId.ToString());

                var configRecord = await configRepository.SingleConfigByIdAsync(filters);

                if (configRecord != null
                    && configRecord.Id > 0
                    && configRecord.ConfigTypeId == WorkflowConfigTypeId
                    && !string.IsNullOrWhiteSpace(configRecord.Contents)
                    && configRecord.Contents != "{}")
                {
                    logWriter?.LogInfo(
                        $"Workflow step edit resolved to workflow '{configRecord.ConfigName}' (id {configRecord.Id}).",
                        caller, nameof(ResolveAsync));

                    return new WorkflowEntryTarget
                    {
                        ConfigId = configRecord.Id,
                        ConfigName = configRecord.ConfigName,
                        Workflow = JsonConvert.DeserializeObject<WorkflowConfig>(configRecord.Contents)
                    };
                }
            }

            logWriter?.LogError(
                $"Workflow step edit arrived with id '{candidateId}', which is not the configs identity of a workflow. Falling back to '{DefaultWorkflowName}'; if the user had a different workflow open, this edit is going to the wrong record.",
                caller, nameof(ResolveAsync));

            return new WorkflowEntryTarget
            {
                ConfigId = 0,
                ConfigName = DefaultWorkflowName,
                Workflow = await workflowAdapter.GetWorkflowAsync(DefaultWorkflowName)
            };
        }

        /// <summary>
        /// Writes the edited workflow back, matched on the configs identity and scoped to the workflow
        /// ConfigType so it cannot overwrite a record of another type that shares the name.
        /// </summary>
        /// <param name="configRepository">Repository used to perform the write.</param>
        /// <returns>A task representing the write.</returns>
        internal async Task SaveAsync(IConfigRepository configRepository)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            await configRepository.SaveWorkflowDesignerConfigAsync(new SaveWorkflowDesignerConfigModel
            {
                ConfigId = ConfigId,
                ConfigName = ConfigName,
                Workflow = JObject.Parse(JsonConvert.SerializeObject(Workflow, settings))
            });
        }
    }
}
