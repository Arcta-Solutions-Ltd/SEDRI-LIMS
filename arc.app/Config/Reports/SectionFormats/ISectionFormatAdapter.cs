using arc.domain.Configuration.ReportsConfig;
using System.Threading.Tasks;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Interface for adapting section format configurations from various sources.
/// </summary>
public interface ISectionFormatAdapter
{
    /// <summary>
    /// Asynchronously retrieves a section format configuration by its name.
    /// Will attempt to locate a config in the cache before falling back to code.
    /// </summary>
    /// <param name="name">The name of the section format to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ReportSectionFormatConfig.</returns>
    Task<ReportSectionFormatConfig> GetFormatAsync(string name);
}
