using arc.domain.Configuration.ReportsConfig;
using System.Threading.Tasks;

namespace arc.app.Config.Reports.DataSection;

/// <summary>
/// Adapter interface for handling data section configurations.
/// </summary>
public interface IDataSectionAdapter
{
    /// <summary>
    /// Asynchronously retrieves the data section configuration for the specified section name.
    /// </summary>
    /// <param name="sectionName">The name of the section.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the data section configuration.</returns>
    Task<DataSectionConfig> GetSectionAsync(string sectionName);
}
