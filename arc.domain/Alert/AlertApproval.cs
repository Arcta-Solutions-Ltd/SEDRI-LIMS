namespace arc.domain.Alert;

/// <summary>
/// Represents an approval or rejection record for an alert.
/// </summary>
public class AlertApproval
{
    /// <summary>
    /// Gets or sets the alert identifier.
    /// </summary>
    public int AlertId { get; set; }

    /// <summary>
    /// Gets or sets the coding status identifier (e.g. Approved, Rejected).
    /// </summary>
    public int CodingStatusId { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who recorded the approval/rejection.
    /// </summary>
    public string RecordedBy { get; set; }
}
