namespace arc.data.model.Event;

/// <summary>
/// Represents the fields in the event table in the database.
/// </summary>
public class EventDataModel : IdBase
{
    /// <summary>
    /// Gets or sets foreign key linking the topic table to the event table.
    /// </summary>
    public int TopicId { get; set; }

    /// <summary>
    /// Gets or sets the name of the event.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the table name associated with the event.
    /// </summary>
    public required string TableName { get; set; }

    /// <summary>
    /// Gets or sets the type of the event.
    /// </summary>
    public required string Type { get; set; }
}
