using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.Config.Workflows;
using System.Linq;
using arc.common.Models.Config;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace arc.app.Configuration.Queries
{
    internal class GetEditWorkflowEntryQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        public GetEditWorkflowEntryQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var workflowAdapter = _serviceProvider.GetService<IWorkflowAdapter>();

            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;

            var workflow = await workflowAdapter.GetWorkflowAsync("SpecimenDefault");

            var step = workflow.GetStep(id);

            var rules = new List<WorkflowRuleModel>();
            var stateRules = new List<StatePassThroughGrid>();

            if (step.ExitState != null && step.ExitState.Options != null)
            {
                foreach (var rule in step.ExitState.Options)
                {
                    var field = rule.Conditions.First();

                    if (field.CurrentState != null)
                    {
                        var stateRule = new StatePassThroughGrid
                        {
                            ExitState = rule.NewState,
                            EntryState = field.CurrentState
                        };
                        stateRules.Add(stateRule);
                    } else
                    {
                        var newRule = new WorkflowRuleModel
                        {
                            ExitState = rule.NewState,
                            Field = field.Field,
                            ListValue = field.Value,
                            NumberValue = field.Value,
                            StringValue = field.Value
                        };
                        rules.Add(newRule);
                    }
                }
            }

            var returnValue = new AddWorkflowEntryModel
            {
                EntryStates = step.EntryState.Replace(" ",""),
                EventField = step.Event,
                DefaultExitStates = step.ExitState != null ? step.ExitState.Default : null,
                WorkflowRuleGrid = new WorkflowRuleGridModel { WorkflowRuleGrid = rules},
                StatePassThroughGrid = stateRules
            };

            if (step.Actions.Options.Count > 0)
            {
                returnValue.Action = "1072";
            }

            return JsonConvert.SerializeObject(returnValue);
        }
    }
}
