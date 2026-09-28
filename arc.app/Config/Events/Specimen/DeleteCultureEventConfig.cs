using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Event configuration for deleting a culture record.
/// Provides metadata such as event name, topic, and table used by the application.
/// </summary>
internal class DeleteCultureEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the Delete Culture event.
    /// - EventName: 'DeleteCulture'
    /// - EventType: 'special'
    /// - Topic: 'Culture'
    /// - TableName: 'Culture'
    /// </summary>
    public string Get()
    {
        return @"{
                    EventName: 'DeleteCulture',
                    Description: '@SpeDelC@',
                    EventType : 'special',
                    Topic : 'Culture',
                    TableName: 'Culture',}";
    }
}
