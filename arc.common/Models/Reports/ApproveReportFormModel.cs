namespace arc.common.Models.Reports;

/// <summary>
/// Model representing the data required to approve a report form.
/// </summary>
public class ApproveReportFormModel
{
    /// <summary>
    /// Gets or sets the unique identifier of this approval record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the display identifier of the report being approved.
    /// </summary>
    public int ReportDisplayId { get; set; }
}
