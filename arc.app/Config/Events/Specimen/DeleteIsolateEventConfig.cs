using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Event configuration for deleting an isolate record.
/// Provides metadata such as event name, topic, table, and type used by the application.
/// </summary>
internal class DeleteIsolateEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the Delete Isolate event.
    /// - EventName: 'DeleteIsolateEvent'
    /// - EventType: 'deletedata'
    /// - Topic: 'Culture'
    /// - TableName: 'Culture'
    /// </summary>
    public string Get()
    {
        return @"{
                    EventName: 'DeleteIsolateEvent',
                    Description: '@SpeDelA@',
                    EventType : 'special',
                    Topic : 'Culture',
                    TableName: 'Culture'
                }";
    }
}
