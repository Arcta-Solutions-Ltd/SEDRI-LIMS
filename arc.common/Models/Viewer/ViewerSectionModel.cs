using System.Collections.Generic;

namespace arc.common.Models.Viewer
{
    public class ViewerSectionModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<ViewerFieldModel> Fields { get; set; }
        public List<ViewerSubSectionModel> SubSections { get; set; }
    }
}
