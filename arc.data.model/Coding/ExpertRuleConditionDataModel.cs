namespace arc.data.model.Coding;

/// <summary>
/// Represents the fields in the expertrulecondition table in the database.
/// Stores conditions that must be met for an expert rule to apply (e.g. antibiotic + susceptibility combinations).
/// </summary>
public class ExpertRuleConditionDataModel : IdAndDateBase
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
    /// Gets or sets foreign key linking the listitem table for test method.
    /// </summary>
    public int? TestMethodId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table for susceptibility result.
    /// </summary>
    public int? SusceptibilityId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table for special consideration.
    /// </summary>
    public int? SpecialConsiderationId { get; set; }

    /// <summary>
    /// Gets or sets the start value for MIC range conditions.
    /// </summary>
    public decimal? StartVal { get; set; }

    /// <summary>
    /// Gets or sets the end value for MIC range conditions.
    /// </summary>
    public decimal? EndVal { get; set; }
}
