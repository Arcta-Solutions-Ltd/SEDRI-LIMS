using arc.common.Models.SystemConfig;
using arc.data.model.Configuration;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface IConfigExtractionUtils
    {
        public ConfigsDataModel Record { get; }

        Task<ListViewConfig> GetViewConfigUsingIdAsync(int id);
        Task SaveViewAsync(ListViewConfig view);
    }
}
