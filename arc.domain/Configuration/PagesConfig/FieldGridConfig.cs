using System.Collections.Generic;

namespace arc.domain.Configuration.PagesConfig
{
    public class FieldGridConfig
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string OptionsName { get; set; }
        // Text to display when a toggle is ON
        public string OnText { get; set; }
        // Text to display when a toggle is OFF
        public string OffText { get; set; }
        public string Mask { get; set; }
        public string Max { get; set; }
        public string Min { get; set; }
        public string MaxDPs { get; set; }
        public string Placeholder { get; set; }
        public string Width { get; set; }
        public string Label { get; set; }
        public bool MultiSelect { get; set; }
        public string GridTitle { get; set; }
        public List<RuleConfig> Rules { get; set; }
        public string FieldFormat { get; set; }
    }
}
