namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// The horizontal space allocated to one area on a layout section row, in points.
/// </summary>
/// <remarks>
/// Produced by <c>ReportSectionLayoutExtensions.ComputeRowSlots</c>. Grid areas use
/// <see cref="Left"/> and <see cref="Width"/> only; field columns also carry a scaled
/// <see cref="LabelWidth"/>.
/// </remarks>
public class ReportSectionLayoutSlotModel
{
    /// <summary>
    /// Gets or sets the distance in points from the left edge of the page to the left edge of the area.
    /// </summary>
    public int Left { get; set; }

    /// <summary>
    /// Gets or sets the total width of the area in points.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the width in points given to the label column of a field column, or zero for a grid.
    /// </summary>
    public int LabelWidth { get; set; }

    /// <summary>
    /// Gets the point at which the area ends, which is the first point beyond its right edge.
    /// </summary>
    public int Right => Left + Width;
}
