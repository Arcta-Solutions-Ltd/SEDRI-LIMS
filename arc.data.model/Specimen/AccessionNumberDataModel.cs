namespace arc.data.model.Specimen;

/// <summary>
/// Represents the fields in the accessionnumber table in the database.
/// </summary>
public class AccessionNumberDataModel : IdBase
{
    /// <summary>
    /// Gets or sets the prefix for the accession number.
    /// </summary>
    public required string Prefix { get; set; }

    /// <summary>
    /// Gets or sets the counter value for the accession number.
    /// </summary>
    public int Counter { get; set; }

    /// <summary>
    /// Gets or sets the category of the accession number.
    /// </summary>
    public string? Category { get; set; }
}
