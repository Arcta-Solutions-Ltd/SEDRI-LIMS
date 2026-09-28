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
/// Event for managing culture file attachments. Replaces the culture's attachments
/// with the set from the form (comma-separated file attachment ids).
/// </summary>
internal class ManageCultureAttachmentsEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    public ManageCultureAttachmentsEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the event: parses form data and calls SetCultureFileAttachmentsAsync.
    /// </summary>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var cultureRepository = _serviceProvider.GetService<ICultureRepository>();
        var logWriter = _serviceProvider.GetService<ILogWriter>();

        logWriter?.LogInfo("ManageCultureAttachments starting", "ManageCultureAttachmentsEvent", "RunAsync");

        try
        {
            var request = JsonConvert.DeserializeObject<ManageAttachmentsRequestModel>(dataToSave);
            var recordId = (request?.Id ?? 0) > 0 ? request.Id : int.TryParse(id, out var n) && n > 0 ? n : 0;
            if (recordId <= 0)
            {
                logWriter?.LogError("ManageCultureAttachments failed: no valid culture id", "ManageCultureAttachmentsEvent", "RunAsync");
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

            await cultureRepository.SetCultureFileAttachmentsAsync(recordId, fileAttachmentIds);
            logWriter?.LogInfo($"ManageCultureAttachments completed cultureId={recordId} count={fileAttachmentIds.Length}", "ManageCultureAttachmentsEvent", "RunAsync");
            return recordId;
        }
        catch (Exception ex)
        {
            logWriter?.LogError($"ManageCultureAttachments failed: {ex.Message}", "ManageCultureAttachmentsEvent", "RunAsync");
            throw;
        }
    }
}
