namespace arc.data.model.Quality;

/// <summary>
/// Represents the fields in the qcorganisms table in the database.
/// </summary>
public class QcOrganismsDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the organism table to the qcorganisms table.
    /// </summary>
    public int OrganismId { get; set; }

    /// <summary>
    /// Gets or sets the standards body for the QC organism.
    /// </summary>
    public string? StandardsBody { get; set; }

    /// <summary>
    /// Gets or sets the primary strain for the QC organism.
    /// </summary>
    public string? PrimaryStrain { get; set; }

    /// <summary>
    /// Gets or sets other strains for the QC organism.
    /// </summary>
    public string? OtherStrains { get; set; }
}
