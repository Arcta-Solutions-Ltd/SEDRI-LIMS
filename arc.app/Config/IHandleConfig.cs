using arc.common.Models;
using System.Threading.Tasks;

namespace arc.app.Config
{
    public interface IHandleConfig
    {
        Task<string> GetConfigurationAsync(TokenInfoModel token, string language);
    }
}
