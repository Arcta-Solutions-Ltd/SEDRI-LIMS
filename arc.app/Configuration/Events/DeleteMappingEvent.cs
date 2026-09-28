using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.common.Utils;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events;
internal class DeleteMappingEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    public DeleteMappingEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
    {
        var jsonUtils = _serviceProvider.GetService<IJsonUtils>();
        var configRepository = _serviceProvider.GetService<IConfigRepository>();

        var configName = jsonUtils.GetSingleFieldValue(dataToSave, "configname");

        await configRepository.DeleteCustomEntryAsync(configName);

        return 0;
    }
}
