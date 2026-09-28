using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents a line element within a report section, defining its position, content, and formatting.
/// </summary>
public class ReportLineModel
{
    /// <summary>
    /// Gets or sets the line number where this element should be positioned.
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// Gets or sets the left margin position for this line element.
    /// </summary>
    public int Left { get; set; }

    /// <summary>
    /// Gets or sets the field reference for this line element.
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// Gets or sets the text content to be displayed on this line.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the font size for the text on this line.
    /// </summary>
    public int FontSize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the text on this line should be displayed in bold.
    /// </summary>
    public bool Bold { get; set; }

    /// <summary>
    /// Gets or sets the calculation expression for this line element.
    /// </summary>
    public string Calc { get; set; }
}
