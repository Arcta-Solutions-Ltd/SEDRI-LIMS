using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Delete Specimen Type Direct Test Option" UI event.
/// </summary>
internal class DeleteSpecimenTypeDirectTestOptionUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Specimen Type Direct Test Option" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action.
    /// It defines the form used for deleting a specimen type direct test option within the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, and the form action tied to the event.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'deletespecimentypedirecttestoptionuievent',
                        description: 'Delete Specimen Type Direct Test Option',
                        type: 'form',
                        action: 'deletespecimentypedirecttestoptionform'
                    }";

        return newEvent;
    }
}
