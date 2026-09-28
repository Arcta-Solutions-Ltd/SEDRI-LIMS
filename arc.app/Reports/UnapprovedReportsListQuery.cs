using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Reports;

/// <summary>
/// Executes a query to retrieve a list of unapproved reports.
/// </summary>
internal class UnapprovedReportsListQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnapprovedReportsListQuery"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    public UnapprovedReportsListQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the query asynchronously to get unapproved reports based on the given filter and token.
    /// </summary>
    /// <param name="queryFilter">Configuration for filtering the query.</param>
    /// <param name="token">Token information for applying security filters.</param>
    /// <returns>A JSON string representing the list of unapproved reports.</returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var reportRepository = _serviceProvider.GetService<IReportRepository>();
        queryFilter.AddString("Approved", "all");
        var reportList = await reportRepository.GetApprovedReportsListAsync(queryFilter);
        return JsonConvert.SerializeObject(reportList);
    }
}
