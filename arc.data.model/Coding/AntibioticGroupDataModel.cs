namespace arc.data.model.Coding;

/// <summary>
/// Represents the fields in the antibioticgroup table in the database.
/// </summary>
public class AntibioticGroupDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets the name of the antibiotic group.
    /// </summary>
    public string? Name { get; set; }
}
