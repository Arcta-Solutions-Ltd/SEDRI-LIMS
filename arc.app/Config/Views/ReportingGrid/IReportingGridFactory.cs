using arc.domain.Configuration.ViewConfig.ReportingGridConfig;
using System.Threading.Tasks;

namespace arc.app.Config.Views.ReportingGrid
{
    public interface IReportingGridFactory
    {
        Task<ReportingGridConfig> GetViewAsync(string viewName);
    }
}
