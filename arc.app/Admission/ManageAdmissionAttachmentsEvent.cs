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

namespace arc.app.Admission;

/// <summary>
/// Event for managing admission file attachments. Replaces the admission's attachments
/// with the set from the form (comma-separated file attachment ids).
/// </summary>
internal class ManageAdmissionAttachmentsEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    public ManageAdmissionAttachmentsEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the event: parses form data and calls SetAdmissionFileAttachmentsAsync.
    /// </summary>
    /// <param name="dataToSave">JSON form data containing Id and FileAttachmentIds.</param>
    /// <param name="id">Record id from context (admission id).</param>
    /// <param name="command">Event command model.</param>
    /// <param name="eventData">Event configuration.</param>
    /// <returns>The admission id on success, or 0 on failure.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var admissionRepository = _serviceProvider.GetService<IAdmissionRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();

        logWriter?.LogInfo("ManageAdmissionAttachments starting", "ManageAdmissionAttachmentsEvent", "RunAsync");

        try
        {
            var request = JsonConvert.DeserializeObject<ManageAttachmentsRequestModel>(dataToSave);
            var recordId = (request?.Id ?? 0) > 0 ? request.Id : int.TryParse(id, out var n) && n > 0 ? n : 0;
            if (recordId <= 0)
            {
                logWriter?.LogError("ManageAdmissionAttachments failed: no valid admission id", "ManageAdmissionAttachmentsEvent", "RunAsync");
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

            await admissionRepository.SetAdmissionFileAttachmentsAsync(recordId, fileAttachmentIds);
            logWriter?.LogInfo($"ManageAdmissionAttachments completed admissionId={recordId} count={fileAttachmentIds.Length}", "ManageAdmissionAttachmentsEvent", "RunAsync");
            return recordId;
        }
        catch (Exception ex)
        {
            logWriter?.LogError($"ManageAdmissionAttachments failed: {ex.Message}", "ManageAdmissionAttachmentsEvent", "RunAsync");
            throw;
        }
    }
}
