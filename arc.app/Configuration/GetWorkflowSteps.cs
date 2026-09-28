using arc.app.Config.Workflows;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Newtonsoft.Json;
using arc.app.Config.Events;
using arc.app.Common;
using System.Linq;

namespace arc.app.Configuration
{
    internal class GetWorkflowSteps : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetWorkflowSteps(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var workflowAdapter = _serviceProvider.GetService<IWorkflowAdapter>();
            var eventAdapter = _serviceProvider.GetService<IEventAdapter>();
            var listRepository = _serviceProvider.GetService<IListRepository>();

            var workflow = await workflowAdapter.GetWorkflowAsync("SpecimenDefault");
            
            var stateList = await listRepository.GetStateListAsync();

            var returnList = new List<object>();

            foreach (var step in workflow.Steps)
            {
                var eventConfig = await eventAdapter.GetEventAsync(step.Event);

                if (eventConfig.Topic.ToLower() != "tests" && eventConfig.Topic.ToLower() != "culturetests")
                {
                    var entryStateList = step.EntryState.Split(",");
                    var entryStates = "";
                    foreach (var state in entryStateList)
                    {
                        if (! string.IsNullOrWhiteSpace(state))
                        {
                            var stateEntry = stateList.First(s => s.Key == state.Trim());
                            if (! string.IsNullOrWhiteSpace(stateEntry.Text))
                            {
                                entryStates = entryStates == "" ? stateEntry.Text : entryStates + ", " + stateEntry.Text;
                            }
                        }
                    }
                    var newItem = new { Id = step.Event, EventName = eventConfig.Description, EntryStates = entryStates };
                    returnList.Add(newItem);
                }
            }

            return JsonConvert.SerializeObject(returnList);
        }
    }
}
