using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Edit Organism Scope Culture Test Option" event.
/// </summary>
internal class EditOrganismScopeCultureTestOptionEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Organism Scope Culture Test Option" event.
    /// </summary>
    /// <returns>
    /// A string representation of the event configuration.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'editOrganismScopeCultureTestOptionEvent',
                        Description: '@LabOrgD@',
                        EventType : 'special',
                        Topic : 'Laboratory',
                        ValidationRules: [
                            { field: 'AssociatedListId', rule: 'required', message: '@ConAE@'}
                        ]
                    }";
    }
}
