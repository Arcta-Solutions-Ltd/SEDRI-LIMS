using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Add Form Specimen Type Option" event.
/// </summary>
internal class AddFormSpecimenTypeOptionEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Form Specimen Type Option" event.
    /// </summary>
    /// <returns>
    /// A string representation of the event configuration, detailing the event name, description, type and
    /// validation rules.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'addformspecimentypeoption',
                        Description: '@ConFormSpeAdd@',
                        EventType : 'specialadddata',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'GroupId', rule: 'required', message: '@BreA@'},
                            { field: 'AssociatedListId', rule: 'required', message: '@ConA@'}
                        ]
                    }";
    }
}
