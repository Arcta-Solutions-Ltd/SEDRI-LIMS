using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Edit Form Specimen Type Option" event.
/// </summary>
internal class EditFormSpecimenTypeOptionEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Form Specimen Type Option" event.
    /// </summary>
    /// <returns>
    /// A string representation of the event configuration, detailing the event name, description, type and
    /// validation rules.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'editformspecimentypeoption',
                        Description: '@ConFormSpeEdi@',
                        EventType : 'special',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'AssociatedListId', rule: 'required', message: '@ConA@'}
                        ]
                    }";
    }
}
