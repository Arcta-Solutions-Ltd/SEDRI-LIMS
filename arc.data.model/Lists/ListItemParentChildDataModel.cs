namespace arc.data.model.Lists;

/// <summary>
/// Represents a data model for a parent-child relationship within a list structure.
/// Inherits common ID and timestamp properties from <see cref="IdAndDateBase"/>.
/// </summary>
public class ListItemParentChildDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the identifier of the parent item.
    /// </summary>
    public int ParentId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the child item.
    /// </summary>
    public int ChildId { get; set; }

    /// <summary>
    /// Gets or sets the display order of the child item under the parent.
    /// </summary>
    public int? DisplayOrder { get; set; }
}

