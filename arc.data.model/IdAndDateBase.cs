namespace arc.data.model;

/// <summary>
/// Represents the id and last modified dates common to all tables in the database.
/// </summary>
public class IdAndDateBase : IdBase
{
    /// <summary>
    /// Gets or sets the last modified date for a table.
    /// </summary>
    [ImmutableOnUpdate]
    public DateTime LastModifiedDate { get; set; }
}
