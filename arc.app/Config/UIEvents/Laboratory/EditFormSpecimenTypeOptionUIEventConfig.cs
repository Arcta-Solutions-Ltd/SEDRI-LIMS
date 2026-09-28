using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Edit Form Specimen Type Option" UI event.
/// </summary>
internal class EditFormSpecimenTypeOptionUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Form Specimen Type Option" UI event.
    /// </summary>
    /// <returns>
    /// A string representation of the event configuration, detailing the form opened by the event.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'editformspecimentypeoptionuievent',
                        description: 'Edit Form Specimen Type Option',
                        type: 'form',
                        action: 'editformspecimentypeoptionform'
                    }";

        return newEvent;
    }
}
