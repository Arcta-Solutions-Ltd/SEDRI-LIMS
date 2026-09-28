using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Add Specimen Type Direct Test Option" UI event.
/// </summary>
internal class AddSpecimenTypeDirectTestOptionUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Specimen Type Direct Test Option" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action.
    /// It defines the form used for adding a new specimen type direct test option within the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, including metadata such as 
    /// the event name, description, and the form action tied to the event.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addspecimentypedirecttestoptionuievent',
                        description: 'Add Specimen Type Direct Test Option',
                        type: 'form',
                        action: 'addspecimentypedirecttestoptionform'
                    }";

        return newEvent;
    }
}

