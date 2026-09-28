using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Delete Specimen Type Direct Test Option" event.
/// </summary>
internal class DeleteSpecimenTypeDirectTestOptionEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Specimen Type Direct Test Option" event.
    /// </summary>
    /// <remarks>
    /// This configuration defines the event's name, description, type, topic, and associated table name.
    /// It specifies the details required to execute the event for deleting specimen type direct test option data.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, and associated table name.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'deleteSpecimenTypeDirectTestOptionEvent',
                        Description: '@LabDelI@',
                        EventType : 'deletedata',
                        Topic : 'Laboratory',
                        TableName: 'LaboratoryConfigs',
                    }";
    }
}
