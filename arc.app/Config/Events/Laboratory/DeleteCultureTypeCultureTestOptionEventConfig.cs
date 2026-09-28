using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Delete Culture Type Culture Test Option" event.
/// </summary>
internal class DeleteCultureTypeCultureTestOptionEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Culture Type Culture Test Option" event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, topic, and associated table name.
    /// It is used to execute the deletion of culture type culture test option data within the laboratory context.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, type, and the associated table name.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'deleteCultureTypeCultureTestOptionEvent',
                        Description: '@LabDelK@',
                        EventType : 'deletedata',
                        Topic : 'Laboratory',
                        TableName: 'LaboratoryConfigs',
                    }";
    }
}

