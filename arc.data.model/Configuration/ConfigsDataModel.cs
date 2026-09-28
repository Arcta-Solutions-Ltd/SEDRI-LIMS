namespace arc.data.model.Configuration;

/// <summary>
/// Represents the fields in the configs table in the database.
/// </summary>
public class ConfigsDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the name of the configuration.
    /// </summary>
    public string? ConfigName { get; set; }
    /// <summary>
    /// Gets or sets foreign key linking the configtype table to the configs table.
    /// </summary>
    public int ConfigTypeId { get; set; }
    /// <summary>
    /// Gets or sets the contents of the configuration for this element in Json format.
    /// </summary>
    [Jsonb]
    public string? Contents { get; set; }
    /// <summary>
    /// Check if the Contents are null or empty.
    /// </summary>
    public bool IsContentsNullOrEmptyJsonString() => (string.IsNullOrEmpty(Contents) || Contents == "{}" || Contents == null);
}
