using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Reports;

/// <summary>
/// Executes a query to retrieve a list of approved reports, applying token-based filters.
/// </summary>
internal class ApprovedReportsListQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApprovedReportsListQuery"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used for dependency resolution.</param>
    public ApprovedReportsListQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the query to retrieve approved report records, using token-scoped filters.
    /// </summary>
    /// <param name="queryFilter">The filter criteria to apply to the report query.</param>
    /// <param name="token">The token providing authorization and contextual data.</param>
    /// <returns>A JSON string containing the list of approved reports.</returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var reportRepository = _serviceProvider.GetService<IReportRepository>();
        queryFilter.AddString("Approved", "Yes");
        var reportList = await reportRepository.GetApprovedReportsListAsync(queryFilter);
        return JsonConvert.SerializeObject(reportList);
    }
}
