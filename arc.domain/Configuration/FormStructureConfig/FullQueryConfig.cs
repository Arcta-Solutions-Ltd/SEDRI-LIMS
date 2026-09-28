using arc.domain.Configuration.MappingsConfig;

namespace arc.domain.Configuration.FormStructureConfig
{
    public class FullQueryConfig : QueryConfig.QueryConfig
    {
        public MapperConfig ResultMapperConfig { get; set; }
        public MapperConfig ParameterMapperConfig { get; set; }
    }
}
