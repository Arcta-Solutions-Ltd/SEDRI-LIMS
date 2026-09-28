using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Defines the configuration for the "Delete Specimen Type Workflow" UI event.
/// </summary>
internal class DeleteSpecimenTypeWorkflowUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the UI event configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the event name, description, type, and associated action.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'deletespecimentypeworkflowuievent',
                        description: 'Delete Specimen Type Workflow',
                        type: 'form',
                        action: 'deletespecimentypeworkflowform'
                    }";

        return newEvent;
    }
}
