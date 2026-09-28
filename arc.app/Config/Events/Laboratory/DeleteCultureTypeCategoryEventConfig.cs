using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Delete Culture Type Category" event.
/// </summary>
internal class DeleteCultureTypeCategoryEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Culture Type Category" event.
    /// </summary>
    /// <remarks>
    /// This configuration defines the event's name, description, type, topic, and the associated table name.
    /// It specifies the details required to execute the event for deleting culture type category data.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, including metadata such as 
    /// the event name, description, and table name.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'deleteculturetypecategoryevent',
                        Description: '@LabDelF@',
                        EventType : 'deletedata',
                        Topic : 'Laboratory',
                        TableName: 'LaboratoryConfigs',
                    }";
    }
}

