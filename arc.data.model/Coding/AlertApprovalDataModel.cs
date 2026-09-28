namespace arc.data.model.Coding;

/// <summary>
/// Represents the fields in the alertapproval table in the database.
/// Stores approval/rejection history for alerts.
/// </summary>
public class AlertApprovalDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the alert table to the alertapproval table.
    /// </summary>
    public int AlertId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the approval or rejection was recorded.
    /// </summary>
    public DateTime DateRecorded { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who recorded the approval or rejection.
    /// </summary>
    public string? RecordedBy { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the alertapproval table.
    /// Links to the CodingApprovalStatus list (Approved/Rejected).
    /// </summary>
    public int? CodingStatusId { get; set; }
}
