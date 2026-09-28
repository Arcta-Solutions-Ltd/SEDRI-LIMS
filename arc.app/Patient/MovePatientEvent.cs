using arc.app.Barcodes;
using arc.app.Common;
using arc.app.Specimen;
using arc.common;
using arc.common.Models.Patient;
using arc.common.Models.Specimen;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Patient
{
    internal class MovePatientEvent : IRun
    {
        public IServiceProvider _serviceProvider;

        public MovePatientEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var specimenRepository = _serviceProvider.GetService<ISpecimenRepository>();
            var logWriter = _serviceProvider.GetService<ILogWriter>();
            var barcodesHandler = _serviceProvider.GetService<IBarcodesHandler>();

            var data = ArcJson.Deserialize<MovePatientModel>(dataToSave);
            if (data.Surname != null && data.PatientId == 0)
            {
                logWriter.LogInfo("Getting the patient barcode", "MovePatientEvent", "RunAsync");
                data.Barcode = await barcodesHandler.GetBarcodeAsync();
            }

            dataToSave = ArcJson.Serialize(data);

            return await specimenRepository.MoveSpecimenAsync(dataToSave);
        }
    }
}
