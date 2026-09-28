using System.Collections.Generic;

namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents a grid within a report section, defining its structure and column headers.
/// </summary>
public class ReportSectionGridModel
{
    /// <summary>
    /// Gets or sets the name of the grid used for identification and reference.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the list of column headers for the grid.
    /// </summary>
    public List<string> Head { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the PDF grid table should render without cell borders.
    /// </summary>
    public bool NoBox { get; set; }
}
