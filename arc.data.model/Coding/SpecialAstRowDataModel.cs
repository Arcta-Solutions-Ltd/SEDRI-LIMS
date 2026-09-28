namespace arc.data.model.Coding;

/// <summary>
/// Represents the fields in the specialastrow table in the database.
/// </summary>
public class SpecialAstRowDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the ast table to the specialastrow table.
    /// </summary>
    public int? AstId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the specialastrow table. This links to the specialtype list in the listitem table.
    /// </summary>
    public int? SpecialTypeId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the specialastrow table. This links to the susceptibility list in the listitem table.
    /// </summary>
    public int? SusceptibilityId { get; set; }

    /// <summary>
    /// Gets or sets whether the result should be displayed on the report.
    /// </summary>
    public string? DisplayOnReport { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the breakpoint table to the specialastrow table.
    /// </summary>
    public int? BreakpointId { get; set; }
}
