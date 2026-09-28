namespace arc.data.model.Lists;

/// <summary>
/// Represents the fields in the namelist table in the database.
/// </summary>
public class NameListDataModel : IdBase
{
    /// <summary>
    /// Gets or sets the name of the name list.
    /// </summary>
    public required string Name { get; set; }
}
