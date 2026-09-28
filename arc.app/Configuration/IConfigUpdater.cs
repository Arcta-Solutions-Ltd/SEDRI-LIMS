using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface IConfigUpdater
    {
        Task Edit(string configName, int configTypeId, string contents);
        Task Delete(string configName);
    }
}
