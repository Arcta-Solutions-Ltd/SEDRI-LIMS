using arc.domain.Configuration.MappingsConfig;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface IProcessMapping
    {
        Task<string> ProcessAsync(MapperConfig config, string message);
    }
}
