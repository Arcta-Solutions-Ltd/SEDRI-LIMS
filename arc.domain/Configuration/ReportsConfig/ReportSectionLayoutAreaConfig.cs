namespace arc.domain.Configuration.ReportsConfig;

/// <summary>
/// One area placed on a layout section row: either the scalar field block or a single bound grid.
/// </summary>
/// <remarks>
/// A grid area references its grid by the stable data section grid id on <see cref="Name"/>, never by
/// description or translated label, so an arrangement survives translation into another language.
/// </remarks>
public class ReportSectionLayoutAreaConfig
{
    /// <summary>
    /// Gets or sets the area kind: <c>Fields</c> for the scalar field block, or <c>Grid</c> for a bound grid.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the stable data section grid id when <see cref="Type"/> is <c>Grid</c>; null for the field block.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets this area's requested share of the row width, 1 to 100. Zero means an equal share.
    /// </summary>
    /// <remarks>
    /// Percentages on a row are normalised when the row geometry is computed, so they never have to add up
    /// to 100. Mixing explicit and zero percentages gives the explicit areas their share first and splits
    /// what is left equally between the rest.
    /// </remarks>
    public int WidthPercent { get; set; }
}
