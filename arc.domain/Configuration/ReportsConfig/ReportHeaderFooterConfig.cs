using System.Collections.Generic;

namespace arc.domain.Configuration.ReportsConfig;
public class ReportHeaderFooterConfig
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int LineSpacing { get; set; } = 4;
    public List<LineConfig> Lines { get; set; } = [];
    public List<ImageConfig> Images { get; set; } = [];
}
