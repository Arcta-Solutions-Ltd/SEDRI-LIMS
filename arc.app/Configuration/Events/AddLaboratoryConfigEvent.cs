using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Utils;
using arc.data.model.Configuration;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events;

/// <summary>
/// Event for adding a new laboratory configuration to the database.
/// </summary>
internal class AddLaboratoryConfigEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string _configName;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddLaboratoryConfigEvent"/> class.
    /// </summary>
    /// <param name="configName">The name of the configuration to be added.</param>
    /// <param name="serviceProvider">The service provider used for resolving dependencies.</param>
    public AddLaboratoryConfigEvent(string configName, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _configName = configName;
    }

    private void LogIfOrganismScopeConfig(int laboratoryId, int configId)
    {
        if (_configName != "organismscopeculturetestoption") return;
        var logWriter = _serviceProvider.GetService<ILogWriter>();
        logWriter?.LogInfo($"Added organism scope culture test option: LaboratoryId={laboratoryId}, ConfigId={configId}", "AddLaboratoryConfigEvent", "RunAsync");
    }

    /// <summary>
    /// Executes the event to add a new laboratory configuration to the database.
    /// </summary>
    /// <param name="dataToSave">The data to be saved in the new configuration record.</param>
    /// <param name="id">The identifier of the laboratory associated with the configuration.</param>
    /// <param name="command">The event model containing event-related details.</param>
    /// <param name="eventData">Optional event configuration providing additional context.</param>
    /// <returns>
    /// An integer result, where the value represents the outcome of the database operation.
    /// </returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var laboratoryConfigRepository = _serviceProvider.GetService<ILaboratoryConfigRepository>();
        var jsonElementRemover = _serviceProvider.GetService<IJsonElementRemover>();
        var jsonReplacer = _serviceProvider.GetService<IJsonReplacer>();

        dataToSave = jsonElementRemover.RemoveElementsByValue(dataToSave, "view");
        dataToSave = jsonElementRemover.RemoveElementsByValue(dataToSave, "event");
        id = jsonReplacer.GetValueInJsonString(dataToSave, "id");

        var newRecord = new LaboratoryConfigsDataModel
        {
            ConfigName = _configName,
            LaboratoryId = int.Parse(id),
            Contents = dataToSave
        };
        var result = await laboratoryConfigRepository.AddAsync(newRecord, "LaboratoryConfigs");
        LogIfOrganismScopeConfig(newRecord.LaboratoryId, result);

        return result;
    }
}
