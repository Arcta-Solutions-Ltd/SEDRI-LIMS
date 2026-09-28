namespace arc.data.model.Export;

/// <summary>
/// Represents the fields in the exportrunhistory table in the database.
/// </summary>
public class ExportRunHistoryDataModel : IdBase
{
    /// <summary>
    /// Gets or sets foreign key linking the exportprofile table to the exportrunhistory table.
    /// </summary>
    public int ExportProfileId { get; set; }

    /// <summary>
    /// Gets or sets the filter criteria used for the export.
    /// </summary>
    public string? Filter { get; set; }

    /// <summary>
    /// Gets or sets the date and time the export was run.
    /// </summary>
    public DateTime RunAt { get; set; }

    /// <summary>
    /// Gets or sets the file attachment identifier for the exported file.
    /// </summary>
    public int? FileAttachmentId { get; set; }

    /// <summary>
    /// Gets or sets the export schedule identifier when the run was triggered by a schedule.
    /// </summary>
    public int? ExportScheduleId { get; set; }
}
