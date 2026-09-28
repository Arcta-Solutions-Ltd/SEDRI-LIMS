using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Utils;
using arc.data.model.Configuration;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;

namespace arc.app.Configuration.Events;
internal class AddMappingEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    public AddMappingEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        var jsonRemover = _serviceProvider.GetService<IJsonElementRemover>();
        var jsonUtils = _serviceProvider.GetService<IJsonUtils>();
        var configRepository = _serviceProvider.GetService<IConfigRepository>();

        dataToSave = jsonRemover.RemoveElementsByValue(dataToSave, "Event");
        dataToSave = jsonRemover.RemoveElementsByValue(dataToSave, "View");

        //Get next available name
        var configName = jsonUtils.GetSingleFieldValue(dataToSave, "Name").Replace(" ", "").ToLower().RemoveSpecialCharacters().Trim();
        if (configName.Length > 57)
        {
            configName = configName[..57];
        }
        var queryFilter = new QueryFilterConfig();
        queryFilter.AddString("Name", configName);
        queryFilter.AddString("Suffix", "mapping");
        var newConfigName = await configRepository.GetNextAvailableNameAsync(queryFilter);
        newConfigName = newConfigName.Trim();

        var newConfig = new ConfigsDataModel
        {
            ConfigTypeId = 22,
            ConfigName = newConfigName,
            Contents = dataToSave
        };

        await configRepository.UpdateCustomEntryAsync(newConfig);

        return 0;

    }
}
