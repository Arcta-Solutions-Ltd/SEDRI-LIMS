using arc.domain.Configuration.ViewConfig.ReportingGridConfig;
using System.Threading.Tasks;

namespace arc.app.Config.Views.ReportingGrid
{
    public class ReportingGridFactory : IReportingGridFactory
    {
        public async Task<ReportingGridConfig> GetViewAsync(string viewName)
        {
            return viewName.ToLower() switch
            {

                "antibiogram" => new AntibiogramReportingGrid().GetView(),
                _ => null
            };
        }
    }
}
