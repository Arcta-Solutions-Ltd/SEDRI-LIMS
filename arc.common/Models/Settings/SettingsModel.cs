using System.Collections.Generic;

namespace arc.common.Models.Settings
{
    public class SettingsModel
    {
        public string Setting { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string EVent { get; set; }
        public string View { get; set; }
        public string TextValue { get; set; }
        public string Id { get; set; }
        public string Enabled { get; set; }
        public string Enabled2 { get; set; }
        public string YearSetting { get; set; }
        public List<SettingsMappingModel> MappingGrid { get; set; }
    }
}
