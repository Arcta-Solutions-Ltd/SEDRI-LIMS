using arc.app.Common;
using arc.app.Reports.InclusionSelectors;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Reports;

internal class CulturePrintSelectorEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    public CulturePrintSelectorEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var reportRepository = _serviceProvider.GetService<IReportRepository>();

        var resultData = JsonConvert.DeserializeObject<CultureDetailsSelectorModel>(dataToSave);

        await reportRepository.UpdateCultureDisplayOnReportAsync(resultData);

        return 0;
    }
}
