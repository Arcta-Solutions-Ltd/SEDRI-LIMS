using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Defines the UI event configuration used for viewing workflow details.
/// </summary>
internal class ViewWorkflowUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the JSON configuration for the 'view workflow' UI event.
    /// The configuration specifies the event name, description, type, and action.
    /// </summary>
    /// <returns>A JSON string defining the UI event for viewing workflow details.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'viewworkflowuievent',
                        description: 'View workflow details',
                        type: 'workflowdesigner',
                        action: 'workflows'
                    }";

        return newEvent;
    }
}
