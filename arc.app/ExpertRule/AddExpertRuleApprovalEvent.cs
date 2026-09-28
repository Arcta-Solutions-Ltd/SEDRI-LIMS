using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.domain.Coding;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

/// <summary>
/// Event for adding an expert rule approval/rejection record.
/// Uses username from the token for RecordedBy.
/// </summary>
internal class AddExpertRuleApprovalEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TokenInfoModel _token;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddExpertRuleApprovalEvent"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency resolution.</param>
    /// <param name="token">The token containing user authentication details.</param>
    /// <param name="logWriter">The log writer instance.</param>
    public AddExpertRuleApprovalEvent(IServiceProvider serviceProvider, TokenInfoModel token, ILogWriter logWriter)
    {
        _serviceProvider = serviceProvider;
        _token = token;
        _logWriter = logWriter;
    }

    /// <summary>
    /// Executes the event asynchronously to add an expert rule approval record.
    /// </summary>
    /// <param name="dataToSave">The JSON string containing ExpertRuleId and CodingStatusId from the form.</param>
    /// <param name="id">Not used for this event.</param>
    /// <param name="command">The event model.</param>
    /// <param name="eventData">Optional configuration data for the event.</param>
    /// <returns>The ID of the newly created expert rule approval record.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var data = JsonConvert.DeserializeObject<ExpertRuleApproval>(dataToSave);
        data.RecordedBy = _token.Username ?? string.Empty;

        _logWriter.LogInfo(
            $"Adding expert rule approval: ExpertRuleId={data.ExpertRuleId}, CodingStatusId={data.CodingStatusId}, RecordedBy={data.RecordedBy}",
            nameof(AddExpertRuleApprovalEvent),
            nameof(RunAsync));

        var expertRuleRepository = _serviceProvider.GetService<IExpertRuleRepository>();
        var approvalId = await expertRuleRepository.AddExpertRuleApprovalAsync(data);

        _logWriter.LogInfo(
            $"Expert rule approval saved: approvalId={approvalId}, ExpertRuleId={data.ExpertRuleId}",
            nameof(AddExpertRuleApprovalEvent),
            nameof(RunAsync));

        return approvalId;
    }
}
