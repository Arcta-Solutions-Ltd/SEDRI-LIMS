using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Defines the configuration for the "Add Specimen Type Workflow" event.
/// </summary>
internal class AddSpecimenTypeWorkflowEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the event name, description, type, topic, and validation rules.</returns>
    public string Get()
    {
        return @"{
                    EventName: 'addSpecimenTypeWorkflowEvent',
                    Description: '@LabAddJ@',
                    EventType : 'specialadddata',
                    Topic : 'Laboratory',
                    ValidationRules: [
                        { field: 'GroupId', rule: 'required', message: '@LabAF@'},
                        { field: 'AssociatedListId', rule: 'required', message: '@BreA@'}
                    ]
                }";
    }
}
