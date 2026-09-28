namespace arc.common.Models.Lists;

/// <summary>
/// Model for displaying a tag in the tags list view.
/// Maps to ListItem rows where ListId=105.
/// </summary>
public class TagListModel
{
    /// <summary>
    /// Gets or sets the list item (tag) identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the tag name (ListItem.Value).
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the parent tag identifier. Null or 0 for root-level tags.
    /// </summary>
    public int? ParentTagId { get; set; }

    /// <summary>
    /// Gets or sets whether the tag is enabled. Used for edit form.
    /// </summary>
    public bool Enabled { get; set; }
}
