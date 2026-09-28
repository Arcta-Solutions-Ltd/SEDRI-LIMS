namespace arc.data.model.Specimen;

/// <summary>
/// Represents the fields in the specimentest table in the database.
/// </summary>
public class SpecimenTestDataModel : IdBase
{
    /// <summary>
    /// Gets or sets foreign key linking the specimen table to the specimentest table.
    /// </summary>
    public int? SpecimenId { get; set; }

    /// <summary>
    /// Gets or sets foreign key linking the testtype table to the specimentest table.
    /// </summary>
    public int? TestTypeId { get; set; }

    /// <summary>
    /// Gets or sets the test results.
    /// </summary>
    public string? Results { get; set; }
}
