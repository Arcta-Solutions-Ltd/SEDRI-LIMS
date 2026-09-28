using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Delete Test Category" event.
/// </summary>
internal class DeleteTestCategoryEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Test Category" event.
    /// </summary>
    /// <remarks>
    /// This configuration defines the event's name, description, type, topic, and associated table name.
    /// It specifies the details required to execute the event for deleting test category data 
    /// within the configuration system.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, including metadata such as 
    /// the event name, description, and the table name.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'deletetestcategoryevent',
                        Description: '@LabDelC@',
                        EventType : 'deletedata',
                        Topic : 'Laboratory',
                        TableName: 'LaboratoryConfigs',
                    }";
    }
}
