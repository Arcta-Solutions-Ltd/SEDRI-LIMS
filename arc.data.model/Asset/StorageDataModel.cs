namespace arc.data.model.Asset;

/// <summary>
/// Represents the data model for storage information.
/// </summary>
public class StorageDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the name of the storage location.
    /// </summary>
    public string? StorageName { get; set; }

    /// <summary>
    /// Gets or sets the fully qualified name of the storage.
    /// </summary>
    public string? FullyQualifiedName { get; set; }

    /// <summary>
    /// Gets or sets the description of the storage location.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the storage type.
    /// </summary>
    public int StorageTypeid { get; set; }

    /// <summary>
    /// Gets or sets the temperature setting of the storage location.
    /// </summary>
    public decimal Temperature { get; set; }

    /// <summary>
    /// Gets or sets the code associated with the storage.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the parent storage location.
    /// </summary>
    public int ParentStorageId { get; set; }

    /// <summary>
    /// Gets or sets additional data related to the storage location.
    /// </summary>
    public string? MoreData { get; set; }

    /// <summary>
    /// Gets or sets the status indicating whether the storage is enabled.
    /// </summary>
    public string? Enabled { get; set; }

    /// <summary>
    /// Gets or sets the laboratoryid thath the storage location is part of.
    /// </summary>
    public int LaboratoryId { get; set; }
}


