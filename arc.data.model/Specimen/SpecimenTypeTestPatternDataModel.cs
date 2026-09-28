namespace arc.data.model.Specimen;

/// <summary>
/// Represents the fields in the specimentypetestpattern table in the database.
/// </summary>
public class SpecimenTypeTestPatternDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the listitem table to the specimentypetestpattern table. This links to the specimentype list in the listitem table.
    /// </summary>
    public int? SpecimenTypeId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the testpattern table to the specimentypetestpattern table.
    /// </summary>
    public int? TestPatternId { get; set; }
}
