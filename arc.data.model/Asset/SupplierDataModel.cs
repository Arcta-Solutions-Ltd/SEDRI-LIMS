namespace arc.data.model.Asset;

/// <summary>
/// Represents the fields in the supplier table in the database.
/// </summary>
public class SupplierDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the code for the supplier.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the name of the supplier.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the first line of the supplier's address.
    /// </summary>
    public string? AddressLine1 { get; set; }

    /// <summary>
    /// Gets or sets the second line of the supplier's address.
    /// </summary>
    public string? AddressLine2 { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the location table to the supplier table.
    /// </summary>
    public int? LocationId { get; set; }

    /// <summary>
    /// Gets or sets the supplier's zip code.
    /// </summary>
    public string? ZipCode { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the supplier table. This links to the supplierstatus list in the listitem table.
    /// </summary>
    public int? SupplierStatusId { get; set; }

    /// <summary>
    /// Gets or sets additional data in JSON format.
    /// </summary>
    [Jsonb]
    public string? MoreData { get; set; }
}
