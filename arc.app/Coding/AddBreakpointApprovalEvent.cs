using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.domain.Coding;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Coding;

/// <summary>
/// Event for adding a breakpoint approval/rejection record.
/// Uses username from the token for RecordedBy.
/// </summary>
internal class AddBreakpointApprovalEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TokenInfoModel _token;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddBreakpointApprovalEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency resolution.</param>
    /// <param name="token">The token containing user authentication details.</param>
    public AddBreakpointApprovalEvent(IServiceProvider serviceProvider, TokenInfoModel token, ILogWriter logWriter)
    {
        _serviceProvider = serviceProvider;
        _token = token;
        _logWriter = logWriter;
    }

    /// <summary>
    /// Executes the event asynchronously to add a breakpoint approval record.
    /// </summary>
    /// <param name="dataToSave">The JSON string containing BreakpointId and CodingStatusId from the form.</param>
    /// <param name="id">Not used for this event.</param>
    /// <param name="command">The event model.</param>
    /// <param name="eventData">Optional configuration data for the event.</param>
    /// <returns>The ID of the newly created breakpoint approval record.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var data = JsonConvert.DeserializeObject<BreakpointApproval>(dataToSave);
        data.RecordedBy = _token.Username ?? string.Empty;

        _logWriter.LogInfo(
            $"Adding breakpoint approval: BreakpointId={data.BreakpointId}, CodingStatusId={data.CodingStatusId}, RecordedBy={data.RecordedBy}",
            nameof(AddBreakpointApprovalEvent),
            nameof(RunAsync));

        var breakpointRepository = _serviceProvider.GetService<IBreakpointRepository>();
        var approvalId = await breakpointRepository.AddBreakpointApprovalAsync(data);

        _logWriter.LogInfo(
            $"Breakpoint approval saved: approvalId={approvalId}, BreakpointId={data.BreakpointId}",
            nameof(AddBreakpointApprovalEvent),
            nameof(RunAsync));

        return approvalId;
    }
}
