namespace arc.domain.Configuration.ListsConfig;

/// <summary>
/// Represents an option configuration typically used for dropdowns or list selections.
/// </summary>
public class OptionsConfig
{
    /// <summary>
    /// Gets or sets the unique key identifier for the option.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets the display text for the option.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the parent key for the option, useful for representing hierarchical data.
    /// </summary>
    public string? ParentKey { get; set; }
}
