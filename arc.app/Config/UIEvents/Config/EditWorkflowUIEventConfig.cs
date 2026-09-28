using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Edit Workflow" UI event.
/// </summary>
internal class EditWorkflowUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Workflow" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action.
    /// It is used to define the form action for editing workflows in the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the UI event configuration, detailing metadata such as 
    /// the event name, description, type, and the form action it triggers.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'editworkflowuievent',
                        description: 'Edit workflow',
                        type: 'form',
                        action: 'editworkflowform'
                    }";

        return newEvent;
    }
}
