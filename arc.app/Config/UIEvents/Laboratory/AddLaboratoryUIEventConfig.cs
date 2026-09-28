using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Add Laboratory" UI event.
/// </summary>
internal class AddLaboratoryUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Laboratory" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action.
    /// It defines the form used for adding a new laboratory.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, including metadata such as 
    /// the event name and the action tied to the form.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addlaboratoryuievent',
                        description: 'Add Laboratory',
                        type: 'form',
                        action: 'addlaboratoryform'
                    }";

        return newEvent;
    }
}
