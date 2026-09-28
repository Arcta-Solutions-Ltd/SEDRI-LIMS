using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Edit Culture Type Culture Test Option" UI event.
/// </summary>
internal class EditCultureTypeCultureTestOptionUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Culture Type Culture Test Option" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action.
    /// It defines the form that will be used for editing a culture type culture test option within the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the UI event configuration, detailing metadata such as 
    /// the event name, description, type, and the form action it triggers.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'editculturetypeculturetestoptionuievent',
                        description: 'Edit Culture Type Culture Test Option',
                        type: 'form',
                        action: 'editculturetypeculturetestoptionform'
                    }";

        return newEvent;
    }
}
