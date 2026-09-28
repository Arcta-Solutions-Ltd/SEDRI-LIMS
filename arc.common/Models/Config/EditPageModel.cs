using System.Collections.Generic;

namespace arc.common.Models.Config
{
    public class EditPageModel : AddPageModel
    {
        public List<FieldListModel> FieldList { get; set; }
    }

    public class FieldListModel
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }
}
