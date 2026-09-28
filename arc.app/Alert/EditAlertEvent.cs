using arc.app.Common;
using arc.common;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using arc.common.Models.Alert;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Alert;

namespace arc.app.Alert
{
    internal class EditAlertEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditAlertEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var alertRepository = _serviceProvider.GetService<IAlertRepository>();
            var mapper = _serviceProvider.GetService<IMapType<AlertDetailsModel, AlertDetails>>();
            var alertDetailsModel = JsonConvert.DeserializeObject<AlertDetailsModel>(dataToSave);
            var extractedModel = mapper.Map(alertDetailsModel);
            return await alertRepository.EditAlertAsync(extractedModel);
        }
    }
}
