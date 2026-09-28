namespace arc.data.model.Asset;

/// <summary>
/// Represents the fields in the suppliercontact table in the database.
/// </summary>
public class SupplierContactDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the supplier table to the suppliercontact table.
    /// </summary>
    public int SupplierId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the contact table to the suppliercontact table.
    /// </summary>
    public int ContactId { get; set; }
}
