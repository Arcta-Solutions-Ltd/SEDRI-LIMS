namespace arc.data.model.Coding;

/// <summary>
/// Represents the fields in the expertruleaction table in the database.
/// Stores actions to apply when an expert rule's conditions are met (e.g. change susceptibility, add comment).
/// </summary>
public class ExpertRuleActionDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the expertrule table.
    /// </summary>
    public int ExpertRuleId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the antibiotic table.
    /// </summary>
    public int? AntibioticId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the antibioticgroup table.
    /// </summary>
    public int? AntibioticGroupId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table for susceptibility result.
    /// </summary>
    public int? SusceptibilityId { get; set; }

    /// <summary>
    /// Gets or sets whether the result should be displayed on the report ('Yes' or 'No').
    /// </summary>
    public string? DisplayOnReport { get; set; }
}
