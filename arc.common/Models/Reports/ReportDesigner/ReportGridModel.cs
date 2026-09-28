namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents a grid configuration within a report section format.
/// Format definitions are layout-only and contain only positioning properties.
/// </summary>
public class ReportGridModel
{
    /// <summary>
    /// Gets or sets the left position of the grid.
    /// </summary>
    public int Left { get; set; }

    /// <summary>
    /// Gets or sets the width of the grid columns as a pipe-separated string (e.g., "240|240|120").
    /// </summary>
    public string Width { get; set; }
}
