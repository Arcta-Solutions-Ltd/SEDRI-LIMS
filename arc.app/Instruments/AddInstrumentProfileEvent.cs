using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using arc.app.SystemConfig;
using arc.domain.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.common.Utils;
using arc.common.Models;
using arc.data.model.Configuration;

namespace arc.app.Instruments
{
    /// <summary>
    /// This class handles the addition of instrument profiles.
    /// </summary>
    internal class AddInstrumentProfileEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TokenInfoModel _token;
        private readonly ILogger<AddInstrumentProfileEvent> _logger;

        /// <summary>
        /// Initializes a new instance of the AddInstrumentProfileEvent class.
        /// </summary>
        /// <param name="serviceProvider">The service provider instance.</param>
        /// <param name="token">The token information model.</param>
        public AddInstrumentProfileEvent(IServiceProvider serviceProvider, TokenInfoModel token)
        {
            _serviceProvider = serviceProvider;
            _token = token;
            _logger = serviceProvider.GetRequiredService<ILogger<AddInstrumentProfileEvent>>();
        }

        /// <summary>
        /// Runs the process of adding an instrument profile asynchronously.
        /// </summary>
        /// <param name="dataToSave">The data to save as a JSON string.</param>
        /// <param name="id">The identifier for the event.</param>
        /// <param name="command">The event model command.</param>
        /// <param name="eventData">Optional event configuration data.</param>
        /// <returns>An integer indicating the result of the operation.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            SingleInstrumentConfig profileDetails = null;
            try
            {
                profileDetails = JsonConvert.DeserializeObject<SingleInstrumentConfig>(dataToSave);
                profileDetails.LaboratoryId = _token.LaboratoryId;

                var configRepository = _serviceProvider.GetService<IConfigRepository>();

                var queryFilter = new QueryFilterConfig();
                queryFilter.AddString("configname", "instrumentinfo");
                var instrumentConfig = await configRepository.SingleConfigByNameAsync(queryFilter);

                var fullConfig = JsonConvert.DeserializeObject<InstrumentConfig>(instrumentConfig.Contents);
                fullConfig.AddInstrument(profileDetails);

                var data = new ConfigsDataModel()
                {
                    Id = instrumentConfig.Id,
                    ConfigName = "instrumentinfo",
                    Contents = ArcJson.Serialize(fullConfig)
                };

                await configRepository.EditCustomEntryAsync(data);

                var criteriaCount = profileDetails.InterfaceCriteria?.Count ?? 0;
                _logger.LogInformation(
                    "Added instrument profile {InstrumentName} (InstrumentMachineId {InstrumentMachineId}, InterfaceTypeId {InterfaceTypeId}, ExportProfileId {ExportProfileId}, InterfaceCriteria {CriteriaCount}) for laboratory {LaboratoryId}; config {ConfigName}.",
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add instrument profile for laboratory {LaboratoryId}.", _token.LaboratoryId);
                throw;
            }
        }
    }
}
