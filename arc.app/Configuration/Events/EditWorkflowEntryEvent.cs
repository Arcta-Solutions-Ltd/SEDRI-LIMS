//using arc.common;
//using arc.domain.Configuration.EventsConfig;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using arc.app.Config.Workflows;
//using arc.app.SystemConfig;
//using Newtonsoft.Json;
//using arc.common.Models.Config;
//using arc.domain.Configuration.WorkflowsConfig;
//using arc.common.Models.SystemConfig;
//using arc.app.Common;

//namespace arc.app.Configuration.Events
//{
//    internal class EditWorkflowEntryEvent : IRun
//    {
//        private readonly IServiceProvider _serviceProvider;

//        public EditWorkflowEntryEvent(IServiceProvider serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//        }

//        public async Task<int> Run(string dataToSave, string id, EventModel command, EventConfig eventData = null)
//        {
//            var workflowAdapter = _serviceProvider.GetService<IWorkflowAdapter>();
//            var configRepository = _serviceProvider.GetService<IConfigRepository>();

//            var workflow = await workflowAdapter.GetWorkflow("SpecimenDefault");

//            var newWorkflowEntry = JsonConvert.DeserializeObject<AddWorkflowEntryModel>(dataToSave);

//            var options = new List<ExitStateConditionConfig>();
//            foreach (var rule in newWorkflowEntry.WorkflowRuleGrid.WorkflowRuleGrid)
//            {
//                var value = string.IsNullOrWhiteSpace(rule.StringValue) ? string.IsNullOrWhiteSpace(rule.NumberValue) ? rule.ListValue : rule.NumberValue : rule.StringValue;
//                var newOption = new ExitStateConditionConfig
//                {
//                    NewState = rule.ExitState,
//                    ConditionType = "and",
//                    Conditions = new List<WorkflowConditionConfig> { new WorkflowConditionConfig { Field = rule.Field, Value = value } }
//                };
//                options.Add(newOption);
//            }

//            foreach (var rule in newWorkflowEntry.StatePassThroughGrid)
//            {
//                var newOption = new ExitStateConditionConfig
//                {
//                    NewState = rule.ExitState,
//                    ConditionType = "and",
//                    Conditions = new List<WorkflowConditionConfig> { new WorkflowConditionConfig { CurrentState = rule.EntryState } }
//                };
//                options.Add(newOption);
//            }

//            var newStep = new StepItemConfig
//            {
//                EntryState = newWorkflowEntry.EntryStates,
//                Event = newWorkflowEntry.EventField,
//                ExitState = new ExitStateConfig
//                {
//                    Default = newWorkflowEntry.DefaultExitStates,
//                    Options = options
//                }
//            };

//            workflow.DeleteStep(newStep.Event);
//            workflow.AddStep(newStep);

//            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
//            var configEntry = new ConfigsModel { ConfigName = "SpecimenDefault", ConfigTypeId = 18, Contents = JsonConvert.SerializeObject(workflow, settings) };
//            await configRepository.UpdateCustomEntryAsync(configEntry);

//            return 0;
//        }
//    }
//}
