using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Barcodes
{
    internal class EditBarcodePrintConfigEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditBarcodePrintConfigEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var barcodeHandler = _serviceProvider.GetService<IBarcodesHandler>();
            await barcodeHandler.EditBarcodePrintConfigAsync("Specimen", dataToSave);
            return int.Parse(id);
        }
    }
}
