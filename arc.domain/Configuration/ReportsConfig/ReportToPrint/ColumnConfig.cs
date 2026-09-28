using System.Collections.Generic;

namespace arc.domain.Configuration.ReportsConfig.ReportToPrint;

/// <summary>
/// One field column of a printable field block: where it sits, how wide it is, and which fields it holds.
/// </summary>
public class ColumnConfig
{
    /// <summary>
    /// The distance in points from the left edge of the page to the left edge of the column.
    /// </summary>
    public int Left { get; set; }

    /// <summary>
    /// The total width of the column in points, covering both the label and the value cell.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// The width in points given to the label cell, with the remainder of <see cref="Width"/> going to the value.
    /// </summary>
    public int LabelWidth { get; set; }

    /// <summary>
    /// The fields rendered in this column, in the order they appear top to bottom.
    /// </summary>
    public List<FieldConfig> Fields { get; set; }
}
