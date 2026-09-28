namespace arc.data.model.Configuration;

/// <summary>
/// Represents the fields in the configshistory table in the database.
/// </summary>
public class ConfigsHistoryDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the configs table to the configshistory table.
    /// </summary>
    public int? ConfigsId { get; set; }

    /// <summary>
    /// Gets or sets the new contents of the configuration in JSON format.
    /// </summary>
    [Jsonb]
    public string? NewContents { get; set; }

    /// <summary>
    /// Gets or sets the old contents of the configuration in JSON format.
    /// </summary>
    [Jsonb]
    public string? OldContents { get; set; }
}
