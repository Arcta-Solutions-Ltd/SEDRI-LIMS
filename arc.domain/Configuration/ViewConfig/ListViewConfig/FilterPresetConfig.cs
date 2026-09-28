using System;
using System.Collections.Generic;

namespace arc.domain.Configuration.ViewConfig.ListViewConfig
{
    /// <summary>
    /// Filter preset configuration. Stored per user in MoreData.FilterPresets.
    /// </summary>
    public class FilterPresetConfig
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public bool Default { get; set; }
        public List<FilterPresetFieldConfig> Fields { get; set; }
        /// <summary>Filter By Keyword text; restored when preset is applied.</summary>
        public string TextSearch { get; set; }
        /// <summary>Start date for date range; restored when preset is applied.</summary>
        public DateTime? StartDate { get; set; }
        /// <summary>End date for date range; restored when preset is applied.</summary>
        public DateTime? EndDate { get; set; }
    }
}
