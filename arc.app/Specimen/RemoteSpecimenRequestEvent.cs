using arc.app.Barcodes;
using arc.app.Common;
using arc.app.Config;
using arc.app.Configuration;
using arc.app.Laboratory;
using arc.app.Settings;
using arc.common;
using arc.common.Models;
using arc.common.Models.Config;
using arc.common.Models.Specimen;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Specimen
{
    public class RemoteSpecimenRequestEvent : IRun
    {
        private readonly ISpecimenRepository _specimenRepository;
        private readonly IBarcodesHandler _barcodesHandler;
        private readonly ILogWriter _logWriter;
        private readonly IAccessionNumberCalculator _accessionNumberCalculator;
        private readonly IListViewConfigFactory _listViewConfigFactory;
        private readonly TokenInfoModel _token;
        private readonly IServiceProvider _serviceProvider;

        public RemoteSpecimenRequestEvent(ISpecimenRepository specimenRepository, IBarcodesHandler barcodesHandler, TokenInfoModel token, ILogWriter logWriter, IAccessionNumberCalculator accessionNumberCalculator, IListViewConfigFactory listViewConfigFactory, IServiceProvider serviceProvider)
        {
            _specimenRepository = specimenRepository;
            _barcodesHandler = barcodesHandler;
            _token = token;
            _logWriter = logWriter;
            _accessionNumberCalculator = accessionNumberCalculator;
            _listViewConfigFactory = listViewConfigFactory;
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var data = JsonConvert.DeserializeObject<CreateSpecimenEventModel>(dataToSave, settings);

            _logWriter.LogInfo("Getting the specimen barcode", "RemoteSpecimenRequestEvent", "Run");
            data.Barcode = await _barcodesHandler.GetBarcodeAsync();

            if (data.Surname != null && data.PatientId == 0)
            {
                _logWriter.LogInfo("Getting the patient barcode", "RemoteSpecimenRequestEvent", "Run");
                data.PatientBarcode = await _barcodesHandler.GetBarcodeAsync();
            }

            _logWriter.LogInfo("Creating the specimen", "RemoteSpecimenRequestEvent", "Run");

        var routingService = _serviceProvider.GetRequiredService<IFormPageTableRoutingService>();
        var tableExceptions = await SpecimenFormSaveRouting.BuildTableExceptionsAsync(
            routingService, dataToSave, eventData);

        _logWriter.LogInfo(
            $"Remote specimen save routing: {tableExceptions.Count} table exception(s) for form",
            "RemoteSpecimenRequestEvent",
            "Run");

            var accessionNumber = data.AccessionNumber;
            if (string.IsNullOrWhiteSpace(accessionNumber))
            {
                accessionNumber = await _accessionNumberCalculator.GetAccessionNumberAsync("accessionnumber", data);
            }

            var patientRef = data.PatientRef;
            if (string.IsNullOrWhiteSpace(patientRef))
            {
                patientRef = await _accessionNumberCalculator.GetAccessionNumberAsync("patientreference", data);
            }

            var viewConfig = eventData.DefaultView != null ? await _listViewConfigFactory.GetViewAsync(eventData.DefaultView) : await _listViewConfigFactory.GetViewAsync(data.View);

            // Load laboratory configuration for culture test defaults
            var laboratoryConfigurationHandler = _serviceProvider.GetService<ILaboratoryConfigurationHandler>();
            await laboratoryConfigurationHandler.LoadSingleLaboratoryConfigurationAsync(data.LaboratoryId);

            _logWriter.LogInfo(
                $"Remote specimen save starting: PatientId={data.PatientId}, newPatient={data.Surname != null && data.PatientId == 0}, AdmissionId={data.AdmissionId}, RequestId={data.RequestId}",
                nameof(RemoteSpecimenRequestEvent),
                nameof(RunAsync));

            var specimenId = await _specimenRepository.CreateSpecimenAsync(data, false, command, _token, tableExceptions, dataToSave, accessionNumber, patientRef, laboratoryConfigurationHandler.LaboratoryConfigurationList);

            _logWriter.LogInfo(
                $"Remote specimen save completed: specimenId={specimenId}",
                nameof(RemoteSpecimenRequestEvent),
                nameof(RunAsync));

            return specimenId;
        }
    }
}
