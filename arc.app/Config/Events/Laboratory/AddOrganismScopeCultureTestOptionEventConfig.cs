using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Add Organism Scope Culture Test Option" event.
/// </summary>
internal class AddOrganismScopeCultureTestOptionEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Organism Scope Culture Test Option" event.
    /// </summary>
    /// <returns>
    /// A string representation of the event configuration.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'addOrganismScopeCultureTestOptionEvent',
                        Description: '@LabOrgC@',
                        EventType : 'specialadddata',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'AssociatedListId', rule: 'required', message: '@ConAE@'}
                        ]
                    }";
    }
}
