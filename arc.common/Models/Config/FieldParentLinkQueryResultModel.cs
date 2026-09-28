using System.Collections.Generic;

namespace arc.common.Models.Config
{
    /// <summary>
    /// Initial query payload for the add/edit field parent-link editor.
    /// </summary>
    public class FieldParentLinkQueryResultModel
    {
        public string ParentList { get; set; }
        public List<PageListFieldOptionModel> PageListFields { get; set; } = [];
        public List<ChildListInfoModel> ChildLists { get; set; } = [];
    }
}
