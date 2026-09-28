using arc.common.Models.Reports.ReportDesigner;
using System.Threading.Tasks;

namespace arc.app.Reports.ReportDesigner;

/// <summary>
/// Populates linked-report counts for header and footer definitions in the report designer.
/// </summary>
public interface IReportHeaderFooterUsageService
{
    /// <summary>
    /// Sets <see cref="ReportSectionModel.LinkedReportCount"/> on each header and footer definition.
    /// </summary>
    /// <param name="model">The designer model whose header and footer definitions will be updated.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PopulateLinkedReportCountsAsync(ReportDesignerModel model);
}
