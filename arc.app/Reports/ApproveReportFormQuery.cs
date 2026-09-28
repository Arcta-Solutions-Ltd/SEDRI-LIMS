using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Reports;

/// <summary>
/// Represents a query runner that retrieves the approval form model and serializes it to JSON.
/// </summary>
internal class ApproveReportFormQuery : IQueryRun
{
    /// <summary>
    /// Provides access to registered services, such as repositories.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of <see cref="ApproveReportFormQuery"/> with the given service provider.
    /// </summary>
    public ApproveReportFormQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the approval form query using the provided filter and token, then returns the result as JSON.
    /// </summary>
    /// <param name="queryFilter">Filtering criteria for the query.</param>
    /// <param name="token">Authentication token information.</param>
    /// <returns>A JSON string representing the <see cref="ApproveReportFormModel"/>.</returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var reportRepository = _serviceProvider.GetService<IReportRepository>();

        var report = await reportRepository.GetApproveReportFormQueryAsync(queryFilter);

        return JsonConvert.SerializeObject(report);
    }
}
