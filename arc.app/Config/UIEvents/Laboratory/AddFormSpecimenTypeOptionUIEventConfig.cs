using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Add Form Specimen Type Option" UI event.
/// </summary>
internal class AddFormSpecimenTypeOptionUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Form Specimen Type Option" UI event.
    /// </summary>
    /// <returns>
    /// A string representation of the event configuration, detailing the form opened by the event.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addformspecimentypeoptionuievent',
                        description: 'Add Form Specimen Type Option',
                        type: 'form',
                        action: 'addformspecimentypeoptionform'
                    }";

        return newEvent;
    }
}
