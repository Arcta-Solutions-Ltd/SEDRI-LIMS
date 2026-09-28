namespace arc.data.model.Asset;

/// <summary>
/// Represents the fields in the inventory table in the database.
/// </summary>
public class InventoryDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the name of the inventory item.
    /// </summary>
    public required string InventoryName { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the inventory table. This links to the inventorytype list in the listitem table.
    /// </summary>
    public int? InventoryTypeId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the laboratory table to the inventory table.
    /// </summary>
    public int? LaboratoryId { get; set; }

    /// <summary>
    /// Gets or sets additional data in JSON format.
    /// </summary>
    [Jsonb]
    public string? MoreData { get; set; }
}
