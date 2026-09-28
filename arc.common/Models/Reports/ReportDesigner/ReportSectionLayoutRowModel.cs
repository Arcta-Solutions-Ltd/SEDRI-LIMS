using System.Collections.Generic;

namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// One horizontal row of a layout section's area arrangement in the report designer.
/// </summary>
public class ReportSectionLayoutRowModel
{
    /// <summary>
    /// Gets or sets the areas on this row, ordered left to right.
    /// </summary>
    public List<ReportSectionLayoutAreaModel> Areas { get; set; } = [];
}
