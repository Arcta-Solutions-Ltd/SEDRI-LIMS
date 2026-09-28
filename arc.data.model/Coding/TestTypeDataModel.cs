namespace arc.data.model.Coding;

/// <summary>
/// Represents the fields in the testtype table in the database.
/// </summary>
public class TestTypeDataModel : IdBase
{
    /// <summary>
    /// Gets or sets the name of the test type.
    /// </summary>
    public string? Name { get; set; }
}
