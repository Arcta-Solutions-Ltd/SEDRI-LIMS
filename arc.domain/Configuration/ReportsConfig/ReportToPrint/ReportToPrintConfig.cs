using System.Collections.Generic;

namespace arc.domain.Configuration.ReportsConfig.ReportToPrint;

public class ReportToPrintConfig
{
    public ReportHeaderFooterConfig Header { get; set; }
    public ReportHeaderFooterConfig Footer { get; set; }
    public List<ContentsConfig> Contents { get; set; } = [];
}
