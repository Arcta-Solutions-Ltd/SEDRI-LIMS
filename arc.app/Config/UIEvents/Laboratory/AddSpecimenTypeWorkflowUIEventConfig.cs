using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Defines the configuration for the "Add Specimen Type Workflow" UI event.
/// </summary>
internal class AddSpecimenTypeWorkflowUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the UI event configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the event name, description, type, and associated action.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addspecimentypeworkflowuievent',
                        description: 'Add Specimen Type Workflow',
                        type: 'form',
                        action: 'addspecimentypeworkflowform'
                    }";

        return newEvent;
    }
}
