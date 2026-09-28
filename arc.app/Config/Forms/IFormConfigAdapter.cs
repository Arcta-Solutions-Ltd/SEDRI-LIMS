using arc.domain.Configuration.FormsConfig;
using System.Threading.Tasks;

namespace arc.app.Config.Forms;

/// <summary>
/// Interface for an adapter responsible for retrieving form configurations.
/// </summary>
public interface IFormConfigAdapter
{
    /// <summary>
    /// Asynchronously retrieves a form configuration based on the specified form name.
    /// </summary>
    /// <param name="formName">The name of the form to retrieve the configuration for.</param>
    /// <returns>A task representing the asynchronous operation, containing the form configuration.</returns>
    Task<FormConfig> GetFormAsync(string formName);
}

