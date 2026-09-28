namespace arc.data.model.Specimen;

/// <summary>
/// Represents the fields in the specimenstatehistory table in the database.
/// </summary>
public class SpecimenStateHistoryDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the specimenstatehistory table. This links to the state list in the listitem table.
    /// </summary>
    public int StateId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the specimen table to the specimenstatehistory table.
    /// </summary>
    public int SpecimenId { get; set; }
}
