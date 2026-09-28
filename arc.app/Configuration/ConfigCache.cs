using arc.app.SystemConfig;
using arc.data.model.Configuration;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration;

/// <summary>
/// A cache to store and retrieve configuration records for improved performance.
/// </summary>
public class ConfigCache : IConfigCache
{
    /// <summary>
    /// Repository for fetching configuration records from the data source.
    /// </summary>
    private readonly IConfigRepository _configRepository;

    /// <summary>
    /// In-memory cache for storing configuration records.
    /// </summary>
    private List<ConfigsDataModel> _configCache = [];

    /// <summary>
    /// Indicates whether the cache has been loaded with data.
    /// </summary>
    private bool _isLoaded = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigCache"/> class.
    /// </summary>
    /// <param name="configRepository">The repository for fetching configuration data.</param>
    public ConfigCache(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }

    /// <summary>
    /// Loads all configuration records into the cache from the repository asynchronously.
    /// </summary>
    public async Task LoadConfig()
    {
        var allConfigs = await _configRepository.AllConfigListAsync();
        _configCache = allConfigs.ToList();
        _isLoaded = true;
    }

    /// <summary>
    /// Retrieves a configuration record from the cache by its name.
    /// </summary>
    /// <param name="name">The name of the configuration to retrieve.</param>
    /// <returns>The configuration record, or null if not found.</returns>
    public ConfigsDataModel GetConfig(string name)
    {
        return _configCache.FirstOrDefault(c => c.ConfigName.Equals(name, System.StringComparison.CurrentCultureIgnoreCase));
    }

    /// <summary>
    /// Indicates whether the cache is loaded.
    /// </summary>
    /// <returns>True if the cache is loaded, otherwise false.</returns>
    public bool isLoaded()
    {
        return _isLoaded;
    }

    /// <summary>
    /// Asynchronously retrieves a configuration record by its name, either from the cache or the repository.
    /// </summary>
    /// <param name="name">The name of the configuration to retrieve.</param>
    /// <returns>The configuration record.</returns>
    public async Task<ConfigsDataModel> GetConfigRecordAsync(string name)
    {
        if (_isLoaded)
        {
            return GetConfig(name);
        }
        else
        {
            var parameters = new QueryFilterConfig { Parameters = [new QueryValuesConfig { Key = "ConfigName", Value = name }] };
            return await _configRepository.SingleConfigByNameAsync(parameters);
        }
    }

    /// <summary>
    /// Retrieves a list of configuration data models that match the specified type.
    /// </summary>
    /// <param name="type">The configuration type ID to filter by.</param>
    /// <returns>
    /// A list of <see cref="ConfigsDataModel"/> objects whose <c>ConfigTypeId</c> equals the specified type.
    /// </returns>
    public List<ConfigsDataModel> GetConfigList(int type)
    {
        return _configCache.Where(c => c.ConfigTypeId == type).ToList();
    }

    /// <summary>
    /// Replaces or inserts a configuration record in the in-memory cache after a database write.
    /// </summary>
    /// <param name="record">The updated configuration record.</param>
    public void UpsertConfig(ConfigsDataModel record)
    {
        if (record == null || string.IsNullOrWhiteSpace(record.ConfigName))
        {
            return;
        }

        var existingIndex = _configCache.FindIndex(c =>
            c.ConfigName.Equals(record.ConfigName, System.StringComparison.CurrentCultureIgnoreCase));

        if (existingIndex >= 0)
        {
            _configCache[existingIndex] = record;
        }
        else
        {
            _configCache.Add(record);
        }

        _isLoaded = true;
    }
}