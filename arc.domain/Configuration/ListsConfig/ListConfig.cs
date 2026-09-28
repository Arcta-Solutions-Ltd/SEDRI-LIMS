using System.Collections.Generic;

namespace arc.domain.Configuration.ListsConfig
{
    public class ListConfig
    {
        public string Name { get; set; }

        public List<OptionsConfig> Options { get; set; }
    }
}
