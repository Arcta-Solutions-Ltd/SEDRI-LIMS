using arc.common.Models.Reports;
using arc.domain.Configuration.ReportsConfig;
using arc.domain.Configuration.ReportsConfig.ReportToPrint;
using System.Threading.Tasks;

namespace arc.app.Reports;

public interface IReportTranslator
{
    Task<ReportToPrintConfig> TranslateAsync(ReportConfig reportConfigInDatabase);
    Task<SectionDefinitionModel> GetSectionDefinitionAsync(string sectionType);
}
