using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Delete Specimen Type Culture Type Option" event.
/// </summary>
internal class DeleteSpecimenTypeCultureTypeOptionEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Specimen Type Culture Type Option" event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, topic, and associated table name.
    /// It is used to execute the deletion of specimen type culture type option data within the laboratory context.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, type, and the associated table name.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'deleteSpecimenTypeCultureTypeOptionEvent',
                        Description: '@LabDelJ@',
                        EventType : 'deletedata',
                        Topic : 'Laboratory',
                        TableName: 'LaboratoryConfigs',
                    }";
    }
}
