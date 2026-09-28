namespace arc.data.model.Configuration;

/// <summary>
/// Data model for storing laboratory configuration details.
/// </summary>
public class LaboratoryConfigsDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the laboratory.
    /// </summary>
    public int LaboratoryId { get; set; }

    /// <summary>
    /// Gets or sets the name of the configuration.
    /// </summary>
    public string? ConfigName { get; set; }

    /// <summary>
    /// Gets or sets the contents of the configuration in JSONB format.
    /// </summary>
    [Jsonb]
    public string? Contents { get; set; }
}
