using arc.common.Models.Config;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface IConfigTransferHandler
    {
        Task<string> GetConfig(ConfigurationExportModel settings);
    }
}
