using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Edit Specimen Type Direct Test Option" UI event.
/// </summary>
internal class EditSpecimenTypeDirectTestOptionUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Specimen Type Direct Test Option" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action.
    /// It defines the form used for editing a specimen type direct test option within the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, and the form action associated with the event.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'editspecimentypedirecttestoptionuievent',
                        description: 'Edit Specimen Type Direct Test Option',
                        type: 'form',
                        action: 'editspecimentypedirecttestoptionform'
                    }";

        return newEvent;
    }
}
