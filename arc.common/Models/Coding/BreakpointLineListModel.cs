namespace arc.common.Models.Coding;

/// <summary>
/// List model for displaying breakpoint lines in the record view embedded list.
/// Susceptibility is resolved from listitem via resultline.resultid.
/// </summary>
public class BreakpointLineListModel
{
    /// <summary>
    /// Gets or sets the result line identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the susceptibility display text (e.g. Susceptible, Intermediate, Resistant)
    /// resolved from listitem via resultline.resultid.
    /// </summary>
    public string SusceptibilityName { get; set; }

    /// <summary>
    /// Gets or sets the start value of the measurement range.
    /// </summary>
    public decimal StartVal { get; set; }

    /// <summary>
    /// Gets or sets the end value of the measurement range.
    /// </summary>
    public decimal EndVal { get; set; }
}
