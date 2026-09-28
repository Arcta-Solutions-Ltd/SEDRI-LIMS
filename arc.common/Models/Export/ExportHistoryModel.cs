using Newtonsoft.Json;
using System;

namespace arc.common.Models.Export;

/// <summary>
/// Model for export history list and record views.
/// </summary>
public class ExportHistoryModel
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("exportprofileid")]
    public int ExportProfileId { get; set; }

    [JsonProperty("exportprofilename")]
    public string? ExportProfileName { get; set; }

    [JsonProperty("runat")]
    public DateTime RunAt { get; set; }

    [JsonProperty("filter")]
    public string? Filter { get; set; }

    [JsonProperty("fileattachmentid")]
    public int? FileAttachmentId { get; set; }

    /// <summary>
    /// Gets or sets the export schedule identifier when the run was triggered by a schedule.
    /// </summary>
    [JsonProperty("exportscheduleid")]
    public int? ExportScheduleId { get; set; }

    /// <summary>
    /// Gets or sets the schedule name when the run was triggered by a schedule.
    /// </summary>
    [JsonProperty("schedulename")]
    public string? ScheduleName { get; set; }
}
