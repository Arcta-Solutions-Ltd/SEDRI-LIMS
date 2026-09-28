using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Reports;

/// <summary>
/// Handles the special query that retrieves a single approved report list entry by its report history ID.
/// Used as the <c>singleQuery</c> for both the Report Approval and View Reports list views,
/// loading the row-level detail required to refresh an individual record after an approve or reject action.
/// </summary>
internal class ApprovedReportListByIdQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initialises a new instance of <see cref="ApprovedReportListByIdQuery"/>.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider used to resolve <see cref="IReportRepository"/> at runtime.
    /// </param>
    public ApprovedReportListByIdQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the query and returns a single <see cref="ApprovedReportsListModel"/> record
    /// serialised as a JSON string.
    /// </summary>
    /// <param name="queryFilter">
    /// Filter configuration that must contain an <c>id</c> integer parameter identifying
    /// the report history record to retrieve. May also contain <c>organisationid</c> and
    /// <c>laboratoryid</c> parameters to restrict the result to records the current user
    /// is permitted to view.
    /// </param>
    /// <param name="token">
    /// The authenticated user's token (not used directly in this query but required by
    /// the <see cref="IQueryRun"/> contract).
    /// </param>
    /// <returns>
    /// A JSON string representing the matching <see cref="ApprovedReportsListModel"/>,
    /// including the <c>ApprovedBy</c> ("Decision By") and <c>ApprovalDate</c> fields.
    /// </returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var reportRepository = _serviceProvider.GetService<IReportRepository>();

        var reportList = await reportRepository.GetApprovedReportsListByIdAsync(queryFilter);

        return JsonConvert.SerializeObject(reportList);
    }
}
