using arc.domain.Configuration.ReportsConfig.ReportToPrint;
using System.Collections.Generic;

namespace arc.domain.Configuration.ReportsConfig;

public class ReportSectionFormatConfig
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public List<LineConfig> Heading { get; set; }
    public List<ColumnConfig> Columns { get; set; }
    public List<GridColumnConfig> Grids { get; set; }
    public List<ImageConfig> Images { get; set; }
}
