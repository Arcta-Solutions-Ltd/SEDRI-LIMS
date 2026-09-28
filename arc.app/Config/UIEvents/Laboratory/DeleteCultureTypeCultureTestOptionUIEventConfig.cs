using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Delete Culture Type Culture Test Option" UI event.
/// </summary>
internal class DeleteCultureTypeCultureTestOptionUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Culture Type Culture Test Option" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action.
    /// It defines the form that will be used for deleting a culture type culture test option within the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the UI event configuration, detailing metadata such as 
    /// the event name, description, type, and the form action it triggers.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'deleteculturetypeculturetestoptionuievent',
                        description: 'Delete Culture Type Culture Test Option',
                        type: 'form',
                        action: 'deleteculturetypeculturetestoptionform'
                    }";

        return newEvent;
    }
}
