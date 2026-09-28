using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Instruments;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using arc.app.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.common.Models.SystemConfig;
using arc.common.Utils;
using arc.data.model.Configuration;

namespace arc.app.Instruments
{
    /// <summary>
    /// Persists changes to an instrument profile in the <c>instrumentinfo</c> config JSON.
    /// </summary>
    internal class EditInstrumentProfileEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TokenInfoModel _token;
        private readonly ILogger<EditInstrumentProfileEvent> _logger;

        public EditInstrumentProfileEvent(IServiceProvider serviceProvider, TokenInfoModel token)
        {
            _serviceProvider = serviceProvider;
            _token = token;
            _logger = serviceProvider.GetRequiredService<ILogger<EditInstrumentProfileEvent>>();
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var profileDetails = JsonConvert.DeserializeObject<SingleInstrumentConfig>(dataToSave);
            profileDetails.LaboratoryId = _token.LaboratoryId;

            var configRepository = _serviceProvider.GetService<IConfigRepository>();

            var queryFilter = new QueryFilterConfig();
            queryFilter.AddString("configname", "instrumentinfo");
            var instrumentConfig = await configRepository.SingleConfigByNameAsync(queryFilter);

            var fullConfig = JsonConvert.DeserializeObject<InstrumentConfig>(instrumentConfig.Contents);
            fullConfig.ReplaceInstrument(profileDetails);

            var data = new ConfigsDataModel()
            {
                Id = instrumentConfig.Id,
                ConfigName = "instrumentinfo",
                Contents = ArcJson.Serialize(fullConfig)
            };

            await configRepository.EditCustomEntryAsync(data);

            var criteriaCount = profileDetails.InterfaceCriteria?.Count ?? 0;
            _logger.LogInformation(
                "Updated instrument profile {InstrumentName} (InstrumentMachineId {InstrumentMachineId}, InterfaceTypeId {InterfaceTypeId}, ExportProfileId {ExportProfileId}, InterfaceCriteria {CriteriaCount}) for laboratory {LaboratoryId}; config {ConfigName}.",
                profileDetails.InstrumentName ?? "(unnamed)",
                profileDetails.InstrumentMachineId ?? "(none)",
                profileDetails.InterfaceTypeId ?? "(none)",
                profileDetails.ExportProfileId ?? "(none)",
                criteriaCount,
                profileDetails.LaboratoryId,
                "instrumentinfo");

            if (profileDetails.InterfaceTypeId == "10" && criteriaCount == 0)
            {
                _logger.LogWarning(
                    "Custom instrument profile {InstrumentName} (ExportProfileId {ExportProfileId}) was saved with no interface criteria; all interface results will match.",
                    profileDetails.InstrumentName ?? "(unnamed)",
                    profileDetails.ExportProfileId ?? "(none)");
            }

            return 0;
        }
    }
}
