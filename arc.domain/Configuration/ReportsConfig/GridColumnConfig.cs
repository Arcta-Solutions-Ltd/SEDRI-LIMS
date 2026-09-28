using arc.domain.Configuration.ReportsConfig.ReportToPrint;
using System.Collections.Generic;

namespace arc.domain.Configuration.ReportsConfig;

public class GridColumnConfig
{
    public int Left { get; set; }
    public string Width { get; set; }
    public int LabelWidth { get; set; }
    public List<FieldConfig> Fields { get; set; }
}
