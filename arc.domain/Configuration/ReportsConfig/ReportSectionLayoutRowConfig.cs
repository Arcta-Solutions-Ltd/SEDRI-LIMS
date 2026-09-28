using System.Collections.Generic;

namespace arc.domain.Configuration.ReportsConfig;

/// <summary>
/// One horizontal row of a layout section. Areas on the same row render side by side, sharing a top edge,
/// and the row occupies the height of its tallest area.
/// </summary>
public class ReportSectionLayoutRowConfig
{
    /// <summary>
    /// Gets or sets the areas on this row, ordered left to right.
    /// </summary>
    public List<ReportSectionLayoutAreaConfig> Areas { get; set; } = [];
}
