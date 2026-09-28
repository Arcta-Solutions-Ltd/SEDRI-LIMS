namespace arc.data.model.Coding;

/// <summary>
/// Represents the fields in the resultline table in the database.
/// </summary>
public class ResultLineDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the result table to the resultline table.
    /// </summary>
    public int? ResultId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the breakpoint table to the resultline table.
    /// </summary>
    public int? BreakpointId { get; set; }

    /// <summary>
    /// Gets or sets the start value for the result line.
    /// </summary>
    public decimal? StartVal { get; set; }

    /// <summary>
    /// Gets or sets the end value for the result line.
    /// </summary>
    public decimal? EndVal { get; set; }
}
