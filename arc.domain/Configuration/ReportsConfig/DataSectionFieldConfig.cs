namespace arc.domain.Configuration.ReportsConfig;

/// <summary>
/// Represents a configuration entry for a labeled data field within a section.
/// </summary>
public class DataSectionFieldConfig
{
    /// <summary>
    /// The display label associated with the data field.
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// The value assigned to the data field.
    /// </summary>
    public string Value { get; set; }
}
