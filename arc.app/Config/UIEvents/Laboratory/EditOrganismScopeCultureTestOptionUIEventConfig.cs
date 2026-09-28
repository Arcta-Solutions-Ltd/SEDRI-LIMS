using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Edit Organism Scope Culture Test Option" UI event.
/// </summary>
internal class EditOrganismScopeCultureTestOptionUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Organism Scope Culture Test Option" UI event.
    /// </summary>
    /// <returns>
    /// A string representation of the UI event configuration.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'editorganismscopeculturetestoptionuievent',
                        description: 'Edit Organism Scope Culture Test Option',
                        type: 'form',
                        action: 'editorganismscopeculturetestoptionform'
                    }";

        return newEvent;
    }
}
