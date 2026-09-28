namespace arc.data.model.Asset;

/// <summary>
/// Represents the fields in the supplierinventory table in the database.
/// </summary>
public class SupplierInventoryDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the supplier table to the supplierinventory table.
    /// </summary>
    public int? SupplierId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the inventory table to the supplierinventory table.
    /// </summary>
    public int? InventoryId { get; set; }
}
