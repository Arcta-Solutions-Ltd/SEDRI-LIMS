using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Delete Specimen Type Culture Type Option" UI event.
/// </summary>
internal class DeleteSpecimenTypeCultureTypeOptionUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Specimen Type Culture Type Option" UI event.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the event's name, description, type, and associated action.
    /// It defines the form used for deleting a specimen type culture type option within the system.
    /// </remarks>
    /// <returns>
    /// A string representation of the event configuration, detailing metadata such as 
    /// the event name, description, and the form action tied to the event.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'deletespecimentypeculturetypeoptionuievent',
                        description: 'Delete Specimen Type Culture Type Option',
                        type: 'form',
                        action: 'deletespecimentypeculturetypeoptionform'
                    }";

        return newEvent;
    }
}
