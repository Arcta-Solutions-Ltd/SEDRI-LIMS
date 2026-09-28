namespace arc.data.model.Asset;

/// <summary>
/// Represents the fields in the storageitem table in the database.
/// </summary>
public class StorageItemDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the storagecontent table to the storageitem table.
    /// </summary>
    public int StorageContentsId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the specimen table to the storageitem table.
    /// </summary>
    public int? SpecimenId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the culture table to the storageitem table.
    /// </summary>
    public int? CultureId { get; set; }

    /// <summary>
    /// Gets or sets the number of the storage item.
    /// </summary>
    public int? Number { get; set; }

    /// <summary>
    /// Gets or sets the barcode of the storage item.
    /// </summary>
    public string? Barcode { get; set; }

    /// <summary>
    /// Gets or sets additional data in JSON format.
    /// </summary>
    [Jsonb]
    public string? MoreData { get; set; }
}
