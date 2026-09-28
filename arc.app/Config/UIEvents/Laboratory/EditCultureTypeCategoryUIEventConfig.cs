using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Edit Culture Type Category" UI event.
/// </summary>
internal class EditCultureTypeCategoryUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Culture Type Category" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action. 
    /// It defines the form used for editing a culture type category within the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, and the form action tied to the event.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'editculturetypecategoryuievent',
                        description: 'Edit culture type category',
                        type: 'form',
                        action: 'editculturetypecategoryform'
                    }";

        return newEvent;
    }
}
