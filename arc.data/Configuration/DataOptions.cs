namespace arc.data.Configuration;

/// <summary>
/// Strongly-typed configuration options for the data layer.
/// </summary>
public class DataOptions
{
    /// <summary>
    /// Connection string for the ARC database.
    /// </summary>
    public string ArcConnection { get; set; }
}
