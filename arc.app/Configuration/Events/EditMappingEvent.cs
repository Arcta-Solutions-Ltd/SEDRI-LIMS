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
internal class EditMappingEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    public EditMappingEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        var jsonRemover = _serviceProvider.GetService<IJsonElementRemover>();
        var jsonUtils = _serviceProvider.GetService<IJsonUtils>();
        var configRepository = _serviceProvider.GetService<IConfigRepository>();

        var configName = jsonUtils.GetSingleFieldValue(dataToSave, "configname");

        dataToSave = jsonRemover.RemoveElementsByValue(dataToSave, "Event");
        dataToSave = jsonRemover.RemoveElementsByValue(dataToSave, "View");
        dataToSave = jsonRemover.RemoveElementsByValue(dataToSave, "configname");

        var newConfig = new ConfigsDataModel
        {
            ConfigTypeId = 22,
            ConfigName = configName,
            Contents = dataToSave
        };

        await configRepository.UpdateCustomEntryAsync(newConfig);

        return 0;

    }
}
