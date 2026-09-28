using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.domain.Alert;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Alert;

/// <summary>
/// Event for adding an alert approval/rejection record.
/// Uses username from the token for RecordedBy.
/// </summary>
internal class AddAlertApprovalEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TokenInfoModel _token;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddAlertApprovalEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency resolution.</param>
    /// <param name="token">The token containing user authentication details.</param>
    public AddAlertApprovalEvent(IServiceProvider serviceProvider, TokenInfoModel token)
    {
        _serviceProvider = serviceProvider;
        _token = token;
    }

    /// <summary>
    /// Executes the event asynchronously to add an alert approval record.
    /// </summary>
    /// <param name="dataToSave">The JSON string containing AlertId and CodingStatusId from the form.</param>
    /// <param name="id">Not used for this event.</param>
    /// <param name="command">The event model.</param>
    /// <param name="eventData">Optional configuration data for the event.</param>
    /// <returns>The ID of the newly created alert approval record.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var data = JsonConvert.DeserializeObject<AlertApproval>(dataToSave);
        data.RecordedBy = _token.Username ?? string.Empty;

        var alertRepository = _serviceProvider.GetService<IAlertRepository>();
        return await alertRepository.AddAlertApprovalAsync(data);
    }
}
