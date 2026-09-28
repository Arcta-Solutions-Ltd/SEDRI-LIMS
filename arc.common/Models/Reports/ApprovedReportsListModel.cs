using System;

namespace arc.common.Models.Reports;

/// <summary>
/// Represents a report that has already been approved.
/// </summary>
public class ApprovedReportsListModel
{
    /// <summary>
    /// Unique identifier for the approved report entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifier assigned to the specimen for tracking.
    /// </summary>
    public string AccessionNumber { get; set; }

    /// <summary>
    /// Patient's surname associated with the specimen.
    /// </summary>
    public string Surname { get; set; }

    /// <summary>
    /// Describes the type of specimen tested in the report.
    /// </summary>
    public string SpecimenType { get; set; }

    /// <summary>
    /// Name of the organization that processed or submitted the report.
    /// </summary>
    public string OrganisationName { get; set; }

    /// <summary>
    /// Date when the report was approved.
    /// </summary>
    public DateTime? ApprovalDate { get; set; }

    /// <summary>
    /// Whether a report has been approved.
    /// </summary>
    public string Approved { get; set; }

    /// <summary>
    /// The approval status ID (122 = Needs Approval, 123 = Approved, 124 = Rejected).
    /// Used by workflows to control button visibility. This property serializes to 'stateid' in JSON,
    /// which is required by the frontend workflow filtering system.
    /// </summary>
    public int StateId { get; set; }

    /// <summary>
    /// Who approved the report.
    /// </summary>
    public string ApprovedBy { get; set; }

    /// <summary>
    /// Date when the report was requested.
    /// </summary>
    public DateTime RequestedDate { get; set; }
}
