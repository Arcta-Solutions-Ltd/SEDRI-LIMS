using arc.common.Models;
using System;

namespace arc.domain.Reports;

/// <summary>
/// Represents a record of a report’s history.
/// </summary>
public class ReportHistory
{
    /// <summary>
    /// Gets or sets the unique identifier for this history entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the associated report.
    /// </summary>
    public int ReportId { get; set; }

    /// <summary>
    /// Gets or sets the print status identifier.
    /// </summary>
    public int PrintStatusId { get; set; }

    /// <summary>
    /// Gets or sets the name assigned to this history entry.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the related specimen.
    /// </summary>
    public int SpecimenId { get; set; }

    /// <summary>
    /// Gets or sets the configuration settings used when the report was generated.
    /// </summary>
    public string ReportConfig { get; set; }

    /// <summary>
    /// Gets or sets the content snapshot of the report for this entry.
    /// </summary>
    public string Contents { get; set; }

    /// <summary>
    /// Gets or sets the date time that the report was approved.
    /// </summary>
    public bool IncludeApprovalInfo{ get; set; }

    /// <summary>
    /// Gets or sets the username of who approved this entry.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this historical report is approved.
    /// </summary>
    public int ReportApprovalId { get; set; }
}
