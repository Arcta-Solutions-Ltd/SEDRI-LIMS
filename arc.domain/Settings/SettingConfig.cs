using System.Collections.Generic;

namespace arc.domain.Settings
{
    public class SettingConfig
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Enabled { get; set; }
        public bool Enabled2 { get; set; }
        public string TextValue { get; set; }
        public string Type { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string ErrorMessage { get; set; }
        public List<SettingMappingConfig> MappingValues { get; set; }
    }


    public class SettingMappingConfig
    {
        public int Type { get; set; }
        public string Value { get; set; }
    }
}
