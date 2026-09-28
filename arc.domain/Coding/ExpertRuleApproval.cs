namespace arc.domain.Coding;

/// <summary>
/// Represents an approval or rejection record for an expert rule.
/// </summary>
public class ExpertRuleApproval
{
    /// <summary>
    /// Gets or sets the expert rule identifier.
    /// </summary>
    public int ExpertRuleId { get; set; }

    /// <summary>
    /// Gets or sets the coding status identifier (e.g. Approved, Rejected).
    /// </summary>
    public int CodingStatusId { get; set; }

    /// <summary>
    /// Gets or sets the username of the user who recorded the approval/rejection.
    /// </summary>
    public string RecordedBy { get; set; }
}
