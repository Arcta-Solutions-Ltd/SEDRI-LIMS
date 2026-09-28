namespace arc.data.model.Event;

/// <summary>
/// Represents the fields in the topictranslation table in the database.
/// </summary>
public class TopicTranslationDataModel : IdBase
{
    /// <summary>
    /// Gets or sets the list item name for the topic translation.
    /// </summary>
    public required string ListItemName { get; set; }

    /// <summary>
    /// Gets or sets the displayed topic name.
    /// </summary>
    public required string DisplayedTopic { get; set; }
}
