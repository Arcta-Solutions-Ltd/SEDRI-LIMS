namespace arc.data.model.Coding;

/// <summary>
/// Represents the fields in the breakpointapproval table in the database.
/// Stores approval/rejection history for breakpoints.
/// </summary>
public class BreakpointApprovalDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the breakpoint table to the breakpointapproval table.
    /// </summary>
    public int BreakpointId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the approval or rejection was recorded.
    /// </summary>
    public DateTime DateRecorded { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who recorded the approval or rejection.
    /// </summary>
    public string? RecordedBy { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the breakpointapproval table.
    /// Links to the CodingApprovalStatus list (Approved/Rejected).
    /// </summary>
    public int? CodingStatusId { get; set; }
}
