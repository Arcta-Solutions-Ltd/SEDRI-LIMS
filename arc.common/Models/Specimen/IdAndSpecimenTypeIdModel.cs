namespace arc.common.Models.Specimen;

/// <summary>
/// Represents a model that contains an identifier and a specimen type identifier.
/// Inherits from <see cref="IdModel"/>.
/// </summary>
public class IdAndSpecimenTypeIdModel : IdModel
{
    /// <summary>
    /// Gets or sets the specimen type identifier.
    /// </summary>
    public int SpecimenTypeId { get; set; }
    public int LaboratoryId { get; set; } = 0;
}
