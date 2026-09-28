using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Defines the configuration for the "Delete Specimen Type Workflow" event.
/// </summary>
internal class DeleteSpecimenTypeWorkflowEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the event name, description, type, topic, and validation rules.</returns>
    public string Get()
    {
        return @"{
                    EventName: 'deleteSpecimenTypeWorkflowEvent',
                    Description: '@LabDelL@',
                    EventType : 'deletedata',
                    Topic : 'Laboratory',
                    TableName: 'LaboratoryConfigs'
                }";
    }
}
