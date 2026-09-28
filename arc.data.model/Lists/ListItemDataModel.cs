namespace arc.data.model.Lists;

/// <summary>
/// Represents the fields in the listitem table in the database.
/// </summary>
public class ListItemDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the list table to the listitem table.
    /// </summary>
    public int ListId { get; set; }
    /// <summary>
    /// Gets or sets the value of the list item.
    /// </summary>
    public string Value { get; set; }
    /// <summary>
    /// Gets or sets whether the listitem value can be updated by the user or is fixed. Set to true if the value cannot be updated. 
    /// </summary>
    public bool Fixed { get; set; }
    /// <summary>
    /// Gets or sets whether the list item is enabled. It must be enabled to be selectable in the UI for the system except in table maintenance.
    /// </summary>
    public bool Enabled { get; set; }
    /// <summary>
    /// Gets or sets the order in which this list item is displayed within its containing list.
    /// </summary>
    public int DisplayOrder { get; set; }
    /// <summary>
    /// Gets or sets whether the list item has been deleted. If true then the list item will never appear in the UI for the system.
    /// </summary>
    public bool Deleted { get; set; }
}
