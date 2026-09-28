using System.Collections.Generic;

namespace arc.domain.Configuration.ReportsConfig.ReportToPrint;

/// <summary>
/// Configuration for a header or footer to be printed on a report.
/// </summary>
public class HeaderFooterConfig
{
    /// <summary>
    /// Name of the header/footer, used to identify and locate the configuration.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the header/footer, used to describe the configuration.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// How many pixels to leave after each line before starting the next.
    /// </summary>
    public int LineSpacing { get; set; } = 4;

    /// <summary>
    /// Collection of line configurations for lines to be printed.
    /// </summary>
    public List<LineConfig> Lines { get; set; } = [];

    /// <summary>
    /// Collection of image configurations for images to be drawn.
    /// </summary>
    public List<ImageConfig> Images { get; set; } = [];
}
