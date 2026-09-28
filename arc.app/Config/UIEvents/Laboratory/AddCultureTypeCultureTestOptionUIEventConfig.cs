using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Add Culture Type Culture Test Option" UI event.
/// </summary>
internal class AddCultureTypeCultureTestOptionUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Culture Type Culture Test Option" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action.
    /// It defines the form that will be used for adding a culture type culture test option within the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the UI event configuration, detailing metadata such as 
    /// the event name, description, type, and the form action it triggers.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addculturetypeculturetestoptionuievent',
                        description: 'Add Culture Type Culture TestOption',
                        type: 'form',
                        action: 'addculturetypeculturetestoptionform'
                    }";

        return newEvent;
    }
}
