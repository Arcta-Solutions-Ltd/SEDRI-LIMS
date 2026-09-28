using arc.domain.Configuration.ViewConfig.Common;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using System.Collections.Generic;

namespace arc.domain.Configuration.ViewConfig.ReportingGridConfig
{
    public class ReportingGridConfig
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string DateSearch { get; set; }
        public bool FilterSearch { get; set; }
        public List<ButtonConfig> Buttons { get; set; }
        public List<FilterConfig> Filters { get; set; }
    }
}
