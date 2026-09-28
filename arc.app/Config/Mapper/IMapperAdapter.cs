using arc.domain.Configuration.MappingsConfig;
using System.Threading.Tasks;

namespace arc.app.Config.Mapper
{
    public interface IMapperAdapter
    {
        Task<MapperConfig> GetMapperAsync(string mapperName);
    }
}
