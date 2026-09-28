using arc.common.Models.Common;
using arc.common.Models.Reports.ReportDesigner;
using System.Threading.Tasks;

namespace arc.app.Reports.ReportDesigner;
public interface IReportDesignerHandler
{
    Task<ReportDesignerModel> GetReportDesignerConfiguration(IdAndNameModel reportId);
}
