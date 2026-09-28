namespace arc.data.model.Coding;

/// <summary>
/// Represents the fields in the expertruleapproval table in the database.
/// Stores approval/rejection history for expert rules.
/// </summary>
public class ExpertRuleApprovalDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the expertrule table to the expertruleapproval table.
    /// </summary>
    public int ExpertRuleId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the approval or rejection was recorded.
    /// </summary>
    public DateTime DateRecorded { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who recorded the approval or rejection.
    /// </summary>
    public string? RecordedBy { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the expertruleapproval table.
    /// Links to the CodingApprovalStatus list (Approved/Rejected).
    /// </summary>
    public int? CodingStatusId { get; set; }
}
