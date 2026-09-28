using System.Collections.Generic;

namespace arc.common.Models.Config
{
    public class SectionModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HeadingText { get; set; }
        public string Format { get; set; }
        public string Source { get; set; }
        public List<SectionFieldSelectorModel> FieldSelector { get; set; }
    }
}
