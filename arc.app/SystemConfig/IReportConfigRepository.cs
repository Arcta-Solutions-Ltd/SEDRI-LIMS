using arc.common.Models.Reports.ReportDesigner;
using arc.domain.Configuration.FormStructureConfig;
using arc.domain.Configuration.ReportsConfig;
using System.Threading.Tasks;

namespace arc.app.SystemConfig
{
    public interface IReportConfigRepository
    {
        Task<int> AddNewReportAsync(FullReportConfig newReport);
        Task<int> DeleteReportAsync(FullReportConfig newReport);
        Task<int> AddNewSectionAsync(ReportSectionConfig newReport);
        Task<int> EditSectionAsync(ReportSectionConfig section);
        Task DeleteSectionAsync(string id);
        /// <summary>
        /// Saves every report designer change in a single transaction.
        /// </summary>
        /// <param name="config">The change set produced by the designer.</param>
        /// <returns>A description of every configuration record written, so the designer can reconcile renames.</returns>
        Task<ReportDesignerSaveResultModel> SaveReportDesignerConfigAsync(SaveReportDesignerConfigModel config);
    }
}
