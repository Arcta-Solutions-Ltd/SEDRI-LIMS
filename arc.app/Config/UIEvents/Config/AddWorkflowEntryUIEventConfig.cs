using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Add Workflow Entry" UI event.
/// </summary>
internal class AddWorkflowEntryUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Workflow Entry" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action.
    /// It is used to define the form action for adding workflow entries in the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the UI event configuration, detailing metadata such as 
    /// the event name, description, type, and the form action it triggers.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addworkflowentryuievent',
                        description: 'Add workflow entry',
                        type: 'form',
                        action: 'addworkflowentryform'
                    }";

        return newEvent;
    }
}
