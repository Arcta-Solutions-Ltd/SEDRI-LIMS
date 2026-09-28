using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Reports;
/// <summary>
/// Executes the report unapproval event by updating the report's approval status to unapproved.
/// </summary>
internal class UnapproveReportEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TokenInfoModel _token;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnapproveReportEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider used to resolve application services such as <see cref="IReportRepository"/>.
    /// </param>
    /// <param name="token">
    /// The token containing user information for auditing the unapproval action.
    /// </param>
    public UnapproveReportEvent(IServiceProvider serviceProvider, TokenInfoModel token)
    {
        _serviceProvider = serviceProvider;
        _token = token;
    }

    /// <summary>
    /// Runs the rejection process for a report:
    /// resolves the report repository, records the rejecting user from the token,
    /// sets the approval state to rejected, and returns a status code.
    /// </summary>
    /// <param name="dataToSave">
    /// The raw event payload data (not used in this implementation).
    /// </param>
    /// <param name="id">
    /// The identifier of the report to reject.
    /// </param>
    /// <param name="command">
    /// The event model containing state transition information.
    /// </param>
    /// <param name="eventData">
    /// Optional configuration for the event execution.
    /// </param>
    /// <returns>
    /// A task that resolves to an integer status code (1 indicates success).
    /// </returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var reportRepository = _serviceProvider.GetService<IReportRepository>();
        await reportRepository.ApprovalChangeAsync(id, _token.Username, "No");
        return 1;
    }
}
