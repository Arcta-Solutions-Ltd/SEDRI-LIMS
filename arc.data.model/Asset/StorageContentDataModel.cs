namespace arc.data.model.Asset;

/// <summary>
/// Represents the fields in the storagecontent table in the database.
/// </summary>
public class StorageContentDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the storage table to the storagecontent table.
    /// </summary>
    public int StorageId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the inventory table to the storagecontent table.
    /// </summary>
    public int InventoryId { get; set; }

    /// <summary>
    /// Gets or sets the number of items in the storage content.
    /// </summary>
    public int? Number { get; set; }

    /// <summary>
    /// Gets or sets additional data in JSON format.
    /// </summary>
    [Jsonb]
    public string? MoreData { get; set; }
}
