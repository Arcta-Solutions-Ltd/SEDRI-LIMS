using arc.app.Common;
using arc.common;
using arc.common.Models.Files;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Request;

/// <summary>
/// Event for managing request file attachments. Replaces the request's attachments
/// with the set from the form (comma-separated file attachment ids).
/// </summary>
internal class ManageRequestAttachmentsEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    public ManageRequestAttachmentsEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the event: parses form data and calls SetRequestFileAttachmentsAsync.
    /// </summary>
    /// <param name="dataToSave">JSON form data containing Id and FileAttachmentIds.</param>
    /// <param name="id">Record id from context (request id).</param>
    /// <param name="command">Event command model.</param>
    /// <param name="eventData">Event configuration.</param>
    /// <returns>The request id on success, or 0 on failure.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var requestRepository = _serviceProvider.GetService<IRequestRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();

        logWriter?.LogInfo("ManageRequestAttachments starting", "ManageRequestAttachmentsEvent", "RunAsync");

        try
        {
            var request = JsonConvert.DeserializeObject<ManageAttachmentsRequestModel>(dataToSave);
            var recordId = (request?.Id ?? 0) > 0 ? request.Id : int.TryParse(id, out var n) && n > 0 ? n : 0;
            if (recordId <= 0)
            {
                logWriter?.LogError("ManageRequestAttachments failed: no valid request id", "ManageRequestAttachmentsEvent", "RunAsync");
                return 0;
            }

            var idsRaw = request?.FileAttachmentIds?.Trim() ?? "";
            var fileAttachmentIds = string.IsNullOrWhiteSpace(idsRaw)
                ? Array.Empty<int>()
                : idsRaw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(s => int.TryParse(s, out var num) && num > 0 ? num : 0)
                    .Where(num => num > 0)
                    .Distinct()
                    .ToArray();

            await requestRepository.SetRequestFileAttachmentsAsync(recordId, fileAttachmentIds);
            logWriter?.LogInfo($"ManageRequestAttachments completed requestId={recordId} count={fileAttachmentIds.Length}", "ManageRequestAttachmentsEvent", "RunAsync");
            return recordId;
        }
        catch (Exception ex)
        {
            logWriter?.LogError($"ManageRequestAttachments failed: {ex.Message}", "ManageRequestAttachmentsEvent", "RunAsync");
            throw;
        }
    }
}
