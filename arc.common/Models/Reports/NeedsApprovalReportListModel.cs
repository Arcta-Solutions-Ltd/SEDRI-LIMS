namespace arc.common.Models.Reports;

/// <summary>
/// Model representing a report entry that is pending approval.
/// </summary>
public class NeedsApprovalReportListModel
{
    /// <summary>
    /// Unique identifier for the report record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The accession number identifying the specimen.
    /// </summary>
    public string AccessionNumber { get; set; }

    /// <summary>
    /// Patient's surname associated with the report.
    /// </summary>
    public string Surname { get; set; }

    /// <summary>
    /// Type of specimen submitted for the report.
    /// </summary>
    public string SpecimenType { get; set; }

    /// <summary>
    /// Name of the organization linked to the specimen or report.
    /// </summary>
    public string OrganisationName { get; set; }

    /// <summary>
    /// Current approval state or workflow status of the report.
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// Timestamp of when the report was last modified or updated.
    /// </summary>
    public string LastModifiedDate { get; set; }
}
