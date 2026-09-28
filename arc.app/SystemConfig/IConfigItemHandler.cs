using System.Threading.Tasks;

namespace arc.app.SystemConfig
{
    public interface IConfigItemHandler
    {
        Task<string> GetSingleItemAsync(string configName);
    }
}
