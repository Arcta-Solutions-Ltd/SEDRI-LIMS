namespace arc.common.Models.Specimen;

/// <summary>
/// Represents a specimen record within a laboratory information system.
/// </summary>
public class SpecimenModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the specimen.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the specimen's type
    /// (e.g., blood, tissue, urine), used to classify the nature of the sample.
    /// </summary>
    public int SpecimenTypeId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the laboratory associated with processing or storing this specimen.
    /// </summary>
    public int LaboratoryId { get; set; }
}
