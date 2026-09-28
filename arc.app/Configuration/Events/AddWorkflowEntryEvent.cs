using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.Config.Workflows;
using Newtonsoft.Json;
using arc.common.Models.Config;
using arc.domain.Configuration.WorkflowsConfig;
using System.Collections.Generic;
using arc.app.SystemConfig;

namespace arc.app.Configuration.Events
{
    /// <summary>
    /// Adds a step to a workflow, or replaces an existing step when the event is used for an edit.
    /// </summary>
    /// <remarks>
    /// The workflow written to is the one the request identifies, resolved by <see cref="WorkflowEntryTarget"/>.
    /// </remarks>
    internal class AddWorkflowEntryEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddWorkflowEntryEvent"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve repositories and the log writer.</param>
        /// <param name="type">"add" to append the step, anything else to replace an existing step with the same event.</param>
        public AddWorkflowEntryEvent(IServiceProvider serviceProvider, string type)
        {
            _serviceProvider = serviceProvider;
            _type = type;
        }

        /// <summary>
        /// Builds the step from the submitted form and writes the workflow back.
        /// </summary>
        /// <param name="dataToSave">The JSON payload of the add or edit workflow entry form.</param>
        /// <param name="id">The record identifier supplied by the form handler.</param>
        /// <param name="command">The event model describing the request.</param>
        /// <param name="eventData">Optional event configuration data, unused here.</param>
        /// <returns>Zero; the caller treats this as a configuration event with no new record id.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var workflowAdapter = _serviceProvider.GetService<IWorkflowAdapter>();
            var configRepository = _serviceProvider.GetService<IConfigRepository>();
            var logWriter = _serviceProvider.GetService<ILogWriter>();

            var newWorkflowEntry = JsonConvert.DeserializeObject<AddWorkflowEntryModel>(dataToSave);

            var target = await WorkflowEntryTarget.ResolveAsync(
                newWorkflowEntry?.Id, configRepository, workflowAdapter, logWriter, nameof(AddWorkflowEntryEvent));
            var workflow = target.Workflow;

            var options = new List<ExitStateConditionConfig>();
            foreach (var rule in newWorkflowEntry.WorkflowRuleGrid.WorkflowRuleGrid)
            {
                var value = string.IsNullOrWhiteSpace(rule.StringValue) ? string.IsNullOrWhiteSpace(rule.NumberValue) ? rule.ListValue : rule.NumberValue : rule.StringValue;

                if (rule.Field != null && value != null && rule.ExitState != null)
                {
                    var newOption = new ExitStateConditionConfig
                    {
                        NewState = rule.ExitState,
                        ConditionType = "and",
                        Conditions = new List<WorkflowConditionConfig> { new WorkflowConditionConfig { Field = rule.Field, Value = value } }
                    };
                    options.Add(newOption);
                }
            }

            foreach (var rule in newWorkflowEntry.StatePassThroughGrid)
            {
                if (rule.ExitState != null && rule.EntryState != null)
                {
                    var newOption = new ExitStateConditionConfig
                    {
                        NewState = rule.ExitState,
                        ConditionType = "and",
                        Conditions = new List<WorkflowConditionConfig> { new WorkflowConditionConfig { CurrentState = rule.EntryState } }
                    };
                    options.Add(newOption);
                }
            }

            var newStep = new StepItemConfig
            {
                EntryState = newWorkflowEntry.EntryStates,
                Event = newWorkflowEntry.EventField,
                ExitState = new ExitStateConfig
                {
                    Default = newWorkflowEntry.DefaultExitStates,
                    Options = options
                }
            };


            if (newWorkflowEntry.Action == "1072")
            {
                var newAction = new ActionConditionConfig { Action = "PublishSpecimenReport", NewState = newWorkflowEntry.DefaultExitStates };
                newStep.AddAction(newAction);
            }

            if (_type == "add")
            {
                workflow.AddStep(newStep);
            } else
            {
                workflow.DeleteStep(newStep.Event);
                workflow.AddStep(newStep);
            }

            logWriter?.LogInfo(
                $"{(_type == "add" ? "Adding" : "Replacing")} step '{newStep.Event}' on workflow '{target.ConfigName}' (id {target.ConfigId}); the workflow now has {workflow.Steps?.Count ?? 0} step(s).",
                nameof(AddWorkflowEntryEvent), nameof(RunAsync));

            await target.SaveAsync(configRepository);

            return 0;
        }
    }
}
