namespace arc.data.model.Specimen;

/// <summary>
/// Represents the fields in the specimentypebreakpoint table in the database.
/// </summary>
public class SpecimenTypeBreakpointDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the specimentypebreakpoint table. This links to the specimentype list in the listitem table.
    /// </summary>
    public int? SpecimenTypeId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the breakpoint table to the specimentypebreakpoint table.
    /// </summary>
    public int? BreakpointId { get; set; }
}
