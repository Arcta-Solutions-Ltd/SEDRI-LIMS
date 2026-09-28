namespace arc.domain.Coding;

/// <summary>
/// Represents an approval or rejection record for a breakpoint.
/// </summary>
public class BreakpointApproval
{
    /// <summary>
    /// Gets or sets the breakpoint identifier.
    /// </summary>
    public int BreakpointId { get; set; }

    /// <summary>
    /// Gets or sets the coding status identifier (e.g. Approved, Rejected).
    /// </summary>
    public int CodingStatusId { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who recorded the approval/rejection.
    /// </summary>
    public string RecordedBy { get; set; }
}
