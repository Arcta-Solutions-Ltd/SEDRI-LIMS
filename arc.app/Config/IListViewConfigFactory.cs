using arc.domain.Configuration.ViewConfig.ListViewConfig;
using System.Threading.Tasks;

namespace arc.app.Config;

/// <summary>
/// Interface for a factory that creates and retrieves ListViewConfig instances.
/// </summary>
public interface IListViewConfigFactory
{
    /// <summary>
    /// Asynchronously retrieves a ListViewConfig instance based on the specified view name.
    /// </summary>
    /// <param name="viewName">The name of the view to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, containing the ListViewConfig instance.</returns>
    Task<ListViewConfig> GetViewAsync(string viewName);
}

