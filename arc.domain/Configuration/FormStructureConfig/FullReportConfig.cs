using arc.domain.Configuration.ReportsConfig;
using System.Collections.Generic;

namespace arc.domain.Configuration.FormStructureConfig
{
    public class FullReportConfig : ReportConfig
    {
        public IEnumerable<ReportSectionConfig> MainSectionsConfig { get; set; }
        public IEnumerable<ReportSectionConfig> OrganismSectionsConfig { get; set; }
        public IEnumerable<ReportSectionConfig> FinalSectionsConfig { get; set; }
        public string Enabled { get; set; }
    }
}
