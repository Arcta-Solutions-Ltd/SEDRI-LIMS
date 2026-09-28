using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Alert;

/// <summary>
/// Executes the alert view query. Loads a single alert by the criteria in the query filter
/// and returns it as JSON for display on the alert record view screen.
/// </summary>
internal class AlertViewQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    public AlertViewQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var alertRepository = _serviceProvider.GetService<IAlertRepository>();

        var alert = await alertRepository.AlertViewByIdQueryAsync(queryFilter);

        return JsonConvert.SerializeObject(alert);
    }
}
