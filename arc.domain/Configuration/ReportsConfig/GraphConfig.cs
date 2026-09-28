using arc.domain.Configuration.ViewConfig.ListViewConfig;
using System.Collections.Generic;

namespace arc.domain.Configuration.ReportsConfig
{
    public class GraphConfig
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Stack { get; set; }
        public string Type { get; set; }
        public List<FilterConfig> Filters { get; set; }
        public List<FilterPresetConfig> FilterPresets { get; set; }
        public string DateSearch { get; set; }
        public string Message { get; set; }
    }
}
