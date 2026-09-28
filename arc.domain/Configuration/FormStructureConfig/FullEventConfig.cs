using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.MappingsConfig;

namespace arc.domain.Configuration.FormStructureConfig
{
    public class FullEventConfig : EventConfig
    {
        public MapperConfig MappingConfig { get; set; }
    }
}
