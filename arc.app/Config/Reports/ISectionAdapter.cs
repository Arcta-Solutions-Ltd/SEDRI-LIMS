using arc.domain.Configuration.ReportsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Config.Reports;
/// <summary>
/// Adapter class for handling report sections configuration.
/// </summary>
public interface ISectionAdapter
{
    /// <summary>
    /// Asynchronously retrieves a report section configuration by its name.
    /// </summary>
    /// <param name="sectionName">The name of the section to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the report section configuration.</returns>
    Task<ReportSectionConfig> GetSectionAsync(string sectionName);
    /// <summary>
    /// Asynchronously retrieves all report section configurations.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of report section configurations.</returns>
    Task<List<ReportSectionConfig>> GetAllSectionsAsync();
}
