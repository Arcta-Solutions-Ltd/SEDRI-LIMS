using arc.app.Common;
using arc.common;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.domain.Configuration.EventsConfig;

namespace arc.app.ExpertRule
{
    internal class DeleteExpertRuleEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteExpertRuleEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var expertRuleRepository = _serviceProvider.GetService<IExpertRuleRepository>();
            await expertRuleRepository.DeleteExpertRuleAsync(id);

            return int.Parse(id);
        }
    }
}
