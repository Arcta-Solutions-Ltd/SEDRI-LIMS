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

namespace arc.app.Patient;

/// <summary>
/// Event for managing patient file attachments. Replaces the patient's attachments
/// with the set from the form (comma-separated file attachment ids).
/// </summary>
internal class ManagePatientAttachmentsEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    public ManagePatientAttachmentsEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the event: parses form data and calls SetPatientFileAttachmentsAsync.
    /// </summary>
    /// <param name="dataToSave">JSON form data containing Id and FileAttachmentIds.</param>
    /// <param name="id">Record id from context (patient id).</param>
    /// <param name="command">Event command model.</param>
    /// <param name="eventData">Event configuration.</param>
    /// <returns>The patient id on success, or 0 on failure.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var patientRepository = _serviceProvider.GetService<IPatientRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();

        logWriter?.LogInfo("ManagePatientAttachments starting", "ManagePatientAttachmentsEvent", "RunAsync");

        try
        {
            var request = JsonConvert.DeserializeObject<ManageAttachmentsRequestModel>(dataToSave);
            var recordId = (request?.Id ?? 0) > 0 ? request.Id : int.TryParse(id, out var n) && n > 0 ? n : 0;
            if (recordId <= 0)
            {
                logWriter?.LogError("ManagePatientAttachments failed: no valid patient id", "ManagePatientAttachmentsEvent", "RunAsync");
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

            await patientRepository.SetPatientFileAttachmentsAsync(recordId, fileAttachmentIds);
            logWriter?.LogInfo($"ManagePatientAttachments completed patientId={recordId} count={fileAttachmentIds.Length}", "ManagePatientAttachmentsEvent", "RunAsync");
            return recordId;
        }
        catch (Exception ex)
        {
            logWriter?.LogError($"ManagePatientAttachments failed: {ex.Message}", "ManagePatientAttachmentsEvent", "RunAsync");
            throw;
        }
    }
}
