using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Delete Form Specimen Type Option" UI event.
/// </summary>
internal class DeleteFormSpecimenTypeOptionUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Form Specimen Type Option" UI event.
    /// </summary>
    /// <returns>
    /// A string representation of the event configuration, detailing the form opened by the event.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'deleteformspecimentypeoptionuievent',
                        description: 'Delete Form Specimen Type Option',
                        type: 'form',
                        action: 'deleteformspecimentypeoptionform'
                    }";

        return newEvent;
    }
}
