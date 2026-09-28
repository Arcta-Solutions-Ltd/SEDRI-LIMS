namespace arc.data.model.Event;

/// <summary>
/// Represents the fields in the topic table in the database.
/// </summary>
public class TopicDataModel : IdBase
{
    /// <summary>
    /// Gets or sets the name of the topic.
    /// </summary>
    public required string Name { get; set; }
}
