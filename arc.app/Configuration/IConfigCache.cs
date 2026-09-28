using arc.data.model.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Configuration;

/// <summary>
/// Interface for a configuration cache that manages and retrieves configuration data.
/// </summary>
public interface IConfigCache
{
    /// <summary>
    /// Asynchronously loads all configuration data into the cache.
    /// </summary>
    Task LoadConfig();

    /// <summary>
    /// Retrieves a configuration record from the cache by its name.
    /// </summary>
    /// <param name="name">The name of the configuration to retrieve.</param>
    /// <returns>The configuration record, or null if not found in the cache.</returns>
    ConfigsDataModel GetConfig(string name);

    /// <summary>
    /// Checks if the configuration cache has been loaded.
    /// </summary>
    /// <returns>True if the cache is loaded, otherwise false.</returns>
    bool isLoaded();

    /// <summary>
    /// Asynchronously retrieves a configuration record by its name, 
    /// either from the cache or directly from the repository if the cache is not loaded.
    /// </summary>
    /// <param name="name">The name of the configuration to retrieve.</param>
    /// <returns>The configuration record.</returns>
    Task<ConfigsDataModel> GetConfigRecordAsync(string name);

    /// <summary>
    /// Retrieves a list of configuration data models that match the specified type.
    /// </summary>
    /// <param name="type">The configuration type identifier used for filtering.</param>
    /// <returns>
    /// A list of <see cref="ConfigsDataModel"/> objects with the matching type identifier.
    /// </returns>
    List<ConfigsDataModel> GetConfigList(int type);

    /// <summary>
    /// Replaces or inserts a configuration record in the in-memory cache after a database write.
    /// </summary>
    /// <param name="record">The updated configuration record.</param>
    void UpsertConfig(ConfigsDataModel record);
}