namespace arc.data.model.Specimen;

/// <summary>
/// Represents the fields in the specimentag table in the database.
/// </summary>
public class SpecimenTagDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the specimen table to the specimentag table.
    /// </summary>
    public int? SpecimenId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the specimentag table.
    /// </summary>
    public int? ListItemId { get; set; }
}
