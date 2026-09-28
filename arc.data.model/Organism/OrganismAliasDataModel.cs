namespace arc.data.model.Organism;

/// <summary>
/// Represents the fields in the organismalias table in the database.
/// </summary>
public class OrganismAliasDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the name of the organism alias.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the organism table to the organismalias table.
    /// </summary>
    public int? OrganismId { get; set; }
}
