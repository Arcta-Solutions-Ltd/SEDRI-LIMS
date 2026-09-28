namespace arc.common.Models.Laboratory;

/// <summary>
/// Represents an entry in the laboratory list.
/// </summary>
public class LaboratoryListModel
{
    /// <summary>
    /// Gets or sets the unique identifier for this laboratory entry.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the display name of the laboratory.
    /// </summary>
    public string LaboratoryName { get; set; }

    /// <summary>
    /// Gets or sets the coding list identifier associated with the laboratory.
    /// </summary>
    public string CodingListId { get; set; }

    /// <summary>
    /// Gets or sets the antibiotic group names for display in the list view.
    /// Resolved from comma-separated listitem IDs stored in the laboratory table.
    /// </summary>
    public string AntibioticGroupIds { get; set; }

    /// <summary>
    /// Gets or sets the language code used by the laboratory (e.g. "en", "fr").
    /// </summary>
    public string Language { get; set; }

    /// <summary>
    /// Gets or sets the approval status for reports.
    /// Typical values: "Pending", "Approved", "Rejected".
    /// </summary>
    public string ApproveReports { get; set; }
}
