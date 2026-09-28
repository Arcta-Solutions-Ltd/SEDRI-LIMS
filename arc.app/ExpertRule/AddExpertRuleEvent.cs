using arc.app.Common;
using arc.common;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using arc.common.Models.Coding;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Coding;

namespace arc.app.ExpertRule
{
    internal class AddExpertRuleEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public AddExpertRuleEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var expertRuleRepository = _serviceProvider.GetService<IExpertRuleRepository>();
            var mapper = _serviceProvider.GetService<IMapType<ExpertRuleDetailsModel, arc.domain.Coding.ExpertRule>>();
            if (mapper == null)
            {
                throw new InvalidOperationException("ExpertRuleDetailsModelToExpertRuleMapper is not registered in dependency injection.");
            }
            var expertRuleDetailsModel = JsonConvert.DeserializeObject<ExpertRuleDetailsModel>(dataToSave);
            var extractedModel = mapper.Map(expertRuleDetailsModel);

            return await expertRuleRepository.AddExpertRuleAsync(extractedModel);
        }
    }
}
