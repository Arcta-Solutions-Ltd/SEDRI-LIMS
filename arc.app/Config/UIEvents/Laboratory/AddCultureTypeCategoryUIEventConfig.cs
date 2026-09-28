using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Add Culture Type Category" UI event.
/// </summary>
internal class AddCultureTypeCategoryUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Culture Type Category" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action.
    /// It defines the form used for adding a new culture type category to the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, and associated form action.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addculturetypecategoryuievent',
                        description: 'Add culture type category',
                        type: 'form',
                        action: 'addculturetypecategoryform'
                    }";

        return newEvent;
    }
}
