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

namespace arc.app.Specimen;

/// <summary>
/// Event for managing specimen file attachments. Replaces the specimen's attachments
/// with the set from the form (comma-separated file attachment ids).
/// </summary>
internal class ManageSpecimenAttachmentsEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    public ManageSpecimenAttachmentsEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the event: parses form data and calls SetSpecimenFileAttachmentsAsync.
    /// </summary>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var specimenRepository = _serviceProvider.GetService<ISpecimenRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();

        logWriter?.LogInfo("ManageSpecimenAttachments starting", "ManageSpecimenAttachmentsEvent", "RunAsync");

        try
        {
            var request = JsonConvert.DeserializeObject<ManageAttachmentsRequestModel>(dataToSave);
            var recordId = (request?.Id ?? 0) > 0 ? request.Id : int.TryParse(id, out var n) && n > 0 ? n : 0;
            if (recordId <= 0)
            {
                logWriter?.LogError("ManageSpecimenAttachments failed: no valid specimen id", "ManageSpecimenAttachmentsEvent", "RunAsync");
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

            await specimenRepository.SetSpecimenFileAttachmentsAsync(recordId, fileAttachmentIds);
            logWriter?.LogInfo($"ManageSpecimenAttachments completed specimenId={recordId} count={fileAttachmentIds.Length}", "ManageSpecimenAttachmentsEvent", "RunAsync");
            return recordId;
        }
        catch (Exception ex)
        {
            logWriter?.LogError($"ManageSpecimenAttachments failed: {ex.Message}", "ManageSpecimenAttachmentsEvent", "RunAsync");
            throw;
        }
    }
}
