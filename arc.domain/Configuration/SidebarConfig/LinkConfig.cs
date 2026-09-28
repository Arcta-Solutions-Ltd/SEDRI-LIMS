using System.Collections.Generic;

namespace arc.domain.Configuration.SidebarConfig
{
    public class LinkConfig
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Key { get; set; }
        public List<LinkConfig> Links { get; set; }
    }
}
