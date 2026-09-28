using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using System.Threading.Tasks;

namespace arc.app.Config;

/// <summary>
/// Interface for a factory that creates and retrieves RecordViewConfig instances.
/// </summary>
public interface IRecordViewConfigFactory
{
    /// <summary>
    /// Asynchronously retrieves a RecordViewConfig instance based on the specified view name.
    /// </summary>
    /// <param name="viewName">The name of the view to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, containing the RecordViewConfig instance.</returns>
    Task<RecordViewConfig> GetViewAsync(string viewName);
}

