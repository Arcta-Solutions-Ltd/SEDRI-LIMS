namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents a column configuration within a report section format.
/// Format definitions are layout-only and contain only positioning properties.
/// </summary>
public class ReportColumnModel
{
    /// <summary>
    /// Gets or sets the left position of the column.
    /// </summary>
    public int Left { get; set; }

    /// <summary>
    /// Gets or sets the width of the column.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Gets or sets the width of the label within the column.
    /// </summary>
    public int LabelWidth { get; set; }
}
