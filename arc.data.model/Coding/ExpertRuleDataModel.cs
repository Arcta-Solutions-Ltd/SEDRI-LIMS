namespace arc.data.model.Coding;

/// <summary>
/// Represents the fields in the expertrule table in the database.
/// Stores expert rule definitions for AST interpretation (e.g. EUCAST/CLSI expert rules).
/// </summary>
public class ExpertRuleDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the display name of the expert rule.
    /// </summary>
    public string? ExpertRuleName { get; set; }

    /// <summary>
    /// Gets or sets the rule text/description.
    /// </summary>
    public string? RuleText { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the ordercat table.
    /// </summary>
    public int? OrderId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the family table.
    /// </summary>
    public int? FamilyId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the organism table.
    /// </summary>
    public int? OrganismId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table for organism group coding.
    /// </summary>
    public int? OrgGroupCodingId { get; set; }

    /// <summary>
    /// Gets or sets the combination rule logic (e.g. AND/OR).
    /// </summary>
    public string? CombinationRule { get; set; }

    /// <summary>
    /// Gets or sets whether the rule is enabled ('Yes' or 'No').
    /// </summary>
    public string? Enabled { get; set; }

    /// <summary>
    /// Gets or sets whether to alert when the rule applies.
    /// </summary>
    public string? AlertOnRule { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the specification table. Replaces legacy SourceId; the specification's guidelinesid references the guidelines listitem.
    /// </summary>
    public int? SpecificationId { get; set; }

    /// <summary>
    /// Gets or sets the tag identifier for the rule.
    /// </summary>
    public string? TagId { get; set; }
}
