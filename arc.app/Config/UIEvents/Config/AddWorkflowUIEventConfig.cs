using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Add Workflow" UI event.
/// </summary>
internal class AddWorkflowUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Workflow" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action.
    /// It is used to define the form action for adding workflows in the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the UI event configuration, detailing metadata such as 
    /// the event name, description, type, and the form action it triggers.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addworkflowuievent',
                        description: 'Add workflow',
                        type: 'form',
                        action: 'addworkflowform'
                    }";

        return newEvent;
    }
}
