namespace arc.common.Models.Lists;

/// <summary>
/// Represents an item within a list, including metadata for display and hierarchy.
/// </summary>
public class ListItemModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the list item.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the display value or label of the item.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the enabled status of the item.
    /// Typically "true" or "false" as a string.
    /// </summary>
    public string Enabled { get; set; }

    /// <summary>
    /// Gets or sets whether the item is fixed and cannot be modified.
    /// Typically "true" or "false" as a string.
    /// </summary>
    public string Fixed { get; set; }

    /// <summary>
    /// Gets or sets the parent identifier(s) for this item.
    /// When multiple parents exist, this is a comma-separated list of IDs.
    /// </summary>
    public string ParentId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the list to which this item belongs.
    /// </summary>
    public int ListId { get; set; }
}
