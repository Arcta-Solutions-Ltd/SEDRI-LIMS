using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Reports;

/// <summary>
/// Executes the report approval event by updating the report's approval status.
/// </summary>
internal class ApproveReportEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TokenInfoModel _token;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApproveReportEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider used to resolve required services such as <see cref="IReportRepository"/>.
    /// </param>
    /// <param name="token">
    /// The token containing user information for auditing the approval action.
    /// </param>
    public ApproveReportEvent(IServiceProvider serviceProvider, TokenInfoModel token)
    {
        _serviceProvider = serviceProvider;
        _token = token;
    }

    /// <summary>
    /// Runs the approval process for a report:
    /// resolves the repository, invokes the approval change, and returns a status code.
    /// </summary>
    /// <param name="dataToSave">
    /// The raw event payload data (not used in this implementation).
    /// </param>
    /// <param name="id">
    /// The identifier of the report to approve.
    /// </param>
    /// <param name="command">
    /// The event model containing state transition details.
    /// </param>
    /// <param name="eventData">
    /// Optional configuration data for this event.
    /// </param>
    /// <returns>
    /// A task that completes with an integer status code (1 indicates success).
    /// </returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var reportRepository = _serviceProvider.GetService<IReportRepository>();
        await reportRepository.ApprovalChangeAsync(id, _token.Username, "Yes");
        return 1;
    }
}
