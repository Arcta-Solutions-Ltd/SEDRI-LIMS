using arc.common.Models.SystemConfig;

namespace arc.app.SystemConfig
{
    public interface IConfigFactoryProcessor
    {
        string GetInternalConfig(ConfigsModel model);
    }
}
