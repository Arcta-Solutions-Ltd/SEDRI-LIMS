using arc.app.Configuration;
using arc.domain.Configuration.FormsConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Config.Forms;

/// <summary>
/// Adapter for retrieving and managing form configurations.
/// </summary>
public class FormConfigAdapter : IFormConfigAdapter
{
    /// <summary>
    /// Factory to create form configurations.
    /// </summary>
    private readonly IFormConfigFactory _formConfigFactory;

    /// <summary>
    /// Cache for storing and retrieving configuration data.
    /// </summary>
    private readonly IConfigCache _configCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormConfigAdapter"/> class.
    /// </summary>
    /// <param name="formConfigFactory">The form configuration factory to use.</param>
    /// <param name="configCache">The configuration cache to use.</param>
    public FormConfigAdapter(IFormConfigFactory formConfigFactory, IConfigCache configCache)
    {
        _formConfigFactory = formConfigFactory;
        _configCache = configCache;
    }

    /// <summary>
    /// Asynchronously retrieves the form configuration for the specified form name.
    /// </summary>
    /// <param name="formName">The name of the form to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the form configuration.
    /// </returns>
    public async Task<FormConfig> GetFormAsync(string formName)
    {
        var configRecord = await _configCache.GetConfigRecordAsync(formName);

        var formDef = configRecord == null || configRecord.Contents == null || configRecord.Contents == "{}"
            ? _formConfigFactory.GetForm(formName)
            : JsonConvert.DeserializeObject<FormConfig>(configRecord.Contents);

        return formDef;
    }
}

