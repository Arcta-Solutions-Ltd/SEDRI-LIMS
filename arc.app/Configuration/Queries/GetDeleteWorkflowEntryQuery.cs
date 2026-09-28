using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using arc.app.Config.Events;

namespace arc.app.Configuration.Queries
{
    internal class GetDeleteWorkflowEntryQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        public GetDeleteWorkflowEntryQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var eventName = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;

            var eventAdapter = _serviceProvider.GetService<IEventAdapter>();
            var eventConfig = await eventAdapter.GetEventAsync(eventName);


            var returnObject = new { Event = eventName, Description = eventConfig.Description };
            return JsonConvert.SerializeObject(returnObject);
        }
    }
}
