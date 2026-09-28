using System.Collections.Generic;

namespace arc.domain.Settings
{
    public class ColourSettingConfig
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Enabled { get; set; }
        public string Type { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string ErrorMessage { get; set; }
        public List<ColourSettingMappingConfig> MappingValues { get; set; }
    }

    public class ColourSettingMappingConfig
    {
        public int Type { get; set; }
        public string Text { get; set; }
        public string Colour { get; set; }
    }
}
