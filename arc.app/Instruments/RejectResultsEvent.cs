using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System;
using System.Threading.Tasks;

namespace arc.app.Instruments;
internal class RejectResultsEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    public RejectResultsEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        return 0;
    }
}
