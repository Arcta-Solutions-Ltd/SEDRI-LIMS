using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Delete Workflow" UI event.
/// </summary>
internal class DeleteWorkflowUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Workflow" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action.
    /// It is used to define the form action for deleting workflows in the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the UI event configuration, detailing metadata such as 
    /// the event name, description, type, and the form action it triggers.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'deleteworkflowuievent',
                        description: 'Delete workflow',
                        type: 'form',
                        action: 'deleteworkflowform'
                    }";

        return newEvent;
    }
}
