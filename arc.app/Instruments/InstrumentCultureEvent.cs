using arc.app.Common;
using arc.common;
using Microsoft.Extensions.DependencyInjection;
using arc.common.Models.Instruments;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;
using arc.common.Utils;

namespace arc.app.Instruments
{
    internal class InstrumentCultureEvent : IRun
    {
        public IServiceProvider _serviceProvider;

        public InstrumentCultureEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var instrumentFactory = _serviceProvider.GetService<IInstrumentFactory>();

            var responseModel = ArcJson.Deserialize<ResponseModel>(dataToSave);
            //var responseModel = JsonConvert.DeserializeObject<ResponseModel>(dataToSave);
            var instrumentProcessor = instrumentFactory.Get("ast");
            var result = await instrumentProcessor.Run(responseModel, null);
            if (!result)
            {
                throw new Exception();
            }
            return 0;
        }
    }
}
