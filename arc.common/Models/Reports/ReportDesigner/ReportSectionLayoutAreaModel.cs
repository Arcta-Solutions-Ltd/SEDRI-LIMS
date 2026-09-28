namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// One draggable area within a layout section row in the report designer.
/// </summary>
public class ReportSectionLayoutAreaModel
{
    /// <summary>
    /// Gets or sets the area kind. See <see cref="LayoutAreaTypes"/>.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the stable data section grid id when <see cref="Type"/> is a grid; null for the field block.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets this area's requested share of the row width, 1 to 100. Zero means an equal share.
    /// </summary>
    public int WidthPercent { get; set; }
}
