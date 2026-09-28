namespace arc.data.model.Laboratory;

/// <summary>
/// Represents the data model for a laboratory.
/// Inherits from IdAndDateBase to include standard ID and timestamp properties.
/// </summary>
public class LaboratoryDataModel : IdAndDateBase
{
    /// <summary>
    /// The name of the laboratory.
    /// Defaults to an empty string if not specified.
    /// </summary>
    public string LaboratoryName { get; set; } = "";

    /// <summary>
    /// The identifier for the language used in the laboratory.
    /// Links to a predefined language configuration.
    /// </summary>
    public int LanguageId { get; set; }

    /// <summary>
    /// The identifier for the coding list associated with the laboratory.
    /// May refer to standardized codes used for classification.
    /// </summary>
    public string CodingListId { get; set; } = "";

    /// <summary>
    /// Comma-separated listitem IDs from the AntibioticGroup list (list id 82).
    /// Used for the antibiotic group multiselect on Add/Edit Laboratory forms.
    /// </summary>
    public string AntibioticGroupIds { get; set; } = "";

    /// <summary>
    /// Comma-separated isolate test names (culture test form names) for resistance-mechanism filtering on AST.
    /// When empty, no extra filter is applied beyond culture-type and organism-scope rules.
    /// </summary>
    public string ResistanceMechanismIsolateTestNames { get; set; } = "";

    /// <summary>
    /// The default workflow identifier for the laboratory.
    /// Specifies the primary workflow used within its processes.
    /// </summary>
    public int DefaultWorkflowId { get; set; }

    /// <summary>
    /// Sets whether reports for a laboratory need to be approved laboratory.
    /// </summary>
    public string ApproveReports { get; set; } = "Yes";

    /// <summary>
    /// When 'Yes', manual susceptibility changes on AST require a canned or free-text reason.
    /// </summary>
    public string RecordSusceptibilityChangeAudit { get; set; } = "No";
}
