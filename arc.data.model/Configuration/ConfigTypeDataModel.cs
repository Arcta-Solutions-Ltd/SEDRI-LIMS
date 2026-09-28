namespace arc.data.model.Configuration;

/// <summary>
/// Represents the fields in the configtype table in the database.
/// </summary>
public class ConfigTypeDataModel : IdBase
{
    /// <summary>
    /// Gets or sets the name of the configuration type.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the type of the configuration.
    /// </summary>
    public string? Type { get; set; }
}
