using arc.domain.Configuration.ListsConfig;

namespace arc.app.Config;

/// <summary>
/// Interface for a factory that creates and retrieves list configurations.
/// </summary>
public interface IListConfigFactory
{
    /// <summary>
    /// Retrieves the list configuration based on the provided list name.
    /// </summary>
    /// <param name="listName">The name of the list configuration to retrieve.</param>
    /// <returns>A <see cref="ListConfig"/> object representing the specified list configuration.</returns>
    ListConfig GetList(string listName);
}

