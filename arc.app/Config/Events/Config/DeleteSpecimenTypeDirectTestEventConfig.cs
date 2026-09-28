using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Delete Specimen Type Direct Test" event.
/// </summary>
internal class DeleteSpecimenTypeDirectTestEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Specimen Type Direct Test" event.
    /// </summary>
    /// <remarks>
    /// This configuration defines the event name, description, type, topic, and table name
    /// associated with deleting specimen type direct test records in the laboratory configuration.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, including its metadata.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'deleteSpecimenTypeDirectTest',
                        Description: '@ConDelE@',
                        EventType : 'deletedata',
                        Topic : 'Laboratory',
                        TableName: 'LaboratoryConfigs',
                    }";
    }
}
