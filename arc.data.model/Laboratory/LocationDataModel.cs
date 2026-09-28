namespace arc.data.model.Laboratory;

/// <summary>
/// Represents the fields in the location table in the database.
/// </summary>
public class LocationDataModel : IdAndDateBase
{
    /// <summary>
    /// Gets or sets foreign key linking the location table to itself (parent location).
    /// </summary>
    public int? ParentLocationId { get; set; }

    /// <summary>
    /// Gets or sets the name of the location.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the fully qualified name of the location.
    /// </summary>
    public string? FullyQualifiedName { get; set; }

    /// <summary>
    /// Gets or sets the code for the location.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the latitude coordinate of the location.
    /// </summary>
    public decimal? Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude coordinate of the location.
    /// </summary>
    public decimal? Longitude { get; set; }

    /// <summary>
    /// Gets or sets additional data in JSON format.
    /// </summary>
    [Jsonb]
    public string? MoreData { get; set; }

    /// <summary>
    /// Gets or sets whether the location is enabled.
    /// </summary>
    public required string Enabled { get; set; }
}
