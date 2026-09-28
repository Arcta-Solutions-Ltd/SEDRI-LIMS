using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Delete Culture Type Culture Test" event.
/// </summary>
internal class DeleteCultureTypeCultureTestEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Culture Type Culture Test" event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, associated table name, and topic.
    /// It is used to execute the deletion of culture type culture test data within the laboratory context.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, event type, and the associated table name and topic.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'deleteculturetypeculturetest',
                        Description: '@ConDelAA@',
                        EventType : 'deletedata',
                        TableName: 'laboratoryconfigs',
                        Topic : 'Laboratory'
                    }";
    }
}
