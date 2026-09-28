using arc.domain.Configuration.MappingsConfig;

namespace arc.app.Config.Mapper
{
    public interface IMapperFactory
    {
        MapperConfig GetMapper(string mappingName);
    }
}
