using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Models.SystemConfig;
using arc.common.Utils;
using arc.data.model.Configuration;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Instruments;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Instruments
{
    internal class DeleteInstrumentProfileEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteInstrumentProfileEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var profileDetails = JsonConvert.DeserializeObject<SingleInstrumentConfig>(dataToSave);

            var configRepository = _serviceProvider.GetService<IConfigRepository>();

            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("configname", "instrumentinfo");
            var instrumentConfig = await configRepository.SingleConfigByNameAsync(queryFilter);

            var fullConfig = JsonConvert.DeserializeObject<InstrumentConfig>(instrumentConfig.Contents);
            var instrumentToDelete = fullConfig.Instruments.FirstOrDefault(i => i.InstrumentName == profileDetails.InstrumentName);
            fullConfig.RemoveInstrument(instrumentToDelete);

            var data = new ConfigsDataModel()
            {
                Id = instrumentConfig.Id,
                ConfigName = "instrumentinfo",
                Contents = ArcJson.Serialize(fullConfig)
            };

            await configRepository.EditCustomEntryAsync(data);

            return 0;
        }
    }
}
