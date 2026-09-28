using System.Collections.Generic;

namespace arc.common.Models.Config
{
    public class EditReportSectionModel
    {
        public string Id { get; set; }
        public List<SectionListModel> SectionList { get; set; }
    }

    public class SectionListModel
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }
}
