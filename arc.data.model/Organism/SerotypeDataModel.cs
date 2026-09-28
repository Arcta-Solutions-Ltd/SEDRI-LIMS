namespace arc.data.model.Organism;

/// <summary>
/// Represents the fields in the serotype table in the database.
/// </summary>
public class SerotypeDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the species table to the serotype table.
    /// </summary>
    public int? SpeciesId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the subspecies table to the serotype table.
    /// </summary>
    public int? SubspeciesId { get; set; }

    /// <summary>
    /// Gets or sets the name of the serotype.
    /// </summary>
    public string? Name { get; set; }
}
