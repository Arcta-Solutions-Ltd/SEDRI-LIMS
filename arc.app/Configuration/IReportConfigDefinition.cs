using arc.domain.Configuration.FormStructureConfig;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    public interface IReportConfigDefinition
    {
        Task<FullReportConfig> LoadReportAsync(string reportName);
        Task<FullReportConfig> ResetNamesForNewReportAsync(FullReportConfig reportToCopy, string reportName);
    }
}
