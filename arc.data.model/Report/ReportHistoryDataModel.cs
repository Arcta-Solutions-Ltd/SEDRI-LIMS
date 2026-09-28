namespace arc.data.model.Report;

/// <summary>
/// Represents the fields in the reporthistory table in the database.
/// </summary>
public class ReportHistoryDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the report table to the reporthistory table.
    /// </summary>
    public int? ReportId { get; set; }

    /// <summary>
    /// Gets or sets the name of the report.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the report configuration in JSON format.
    /// </summary>
    [Jsonb]
    public string? ReportConfig { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the specimen table to the reporthistory table.
    /// </summary>
    public int? SpecimenId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the reporthistory table. This links to the printstatus list in the listitem table.
    /// </summary>
    public int? PrintStatusId { get; set; }

    /// <summary>
    /// Gets or sets the contents of the report.
    /// </summary>
    public string? Contents { get; set; }

    /// <summary>
    /// Gets or sets the date the report was approved.
    /// </summary>
    public DateTime? ApprovalDate { get; set; }

    /// <summary>
    /// Gets or sets the user who approved the report.
    /// </summary>
    public string? ApprovedBy { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the reporthistory table. This links to the approval status list in the listitem table.
    /// </summary>
    public int? ReportApprovalId { get; set; }
}
