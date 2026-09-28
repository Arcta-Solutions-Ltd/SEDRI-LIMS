using System.Collections.Generic;

namespace arc.common.Models.Viewer
{
    public class ViewerSubSectionModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<ViewerFieldModel> Fields { get; set; }
    }
}
