using arc.domain.Configuration.PagesConfig;
using System.Threading.Tasks;

namespace arc.app.Config.Pages;

/// <summary>
/// Interface for an adapter responsible for retrieving page configurations.
/// </summary>
public interface IPageConfigAdapter
{
    /// <summary>
    /// Asynchronously retrieves a page configuration based on the specified page name.
    /// </summary>
    /// <param name="pageName">The name of the page to retrieve the configuration for.</param>
    /// <returns>A task representing the asynchronous operation, containing the page configuration.</returns>
    Task<PageConfig> GetPageAsync(string pageName);
}

