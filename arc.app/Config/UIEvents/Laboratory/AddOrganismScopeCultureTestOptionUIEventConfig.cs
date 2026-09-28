using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Add Organism Scope Culture Test Option" UI event.
/// </summary>
internal class AddOrganismScopeCultureTestOptionUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Organism Scope Culture Test Option" UI event.
    /// </summary>
    /// <returns>
    /// A string representation of the UI event configuration.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addorganismscopeculturetestoptionuievent',
                        description: 'Add Organism Scope Culture Test Option',
                        type: 'form',
                        action: 'addorganismscopeculturetestoptionform'
                    }";

        return newEvent;
    }
}
