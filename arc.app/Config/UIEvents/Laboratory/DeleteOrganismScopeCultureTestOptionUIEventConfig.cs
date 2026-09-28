using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Delete Organism Scope Culture Test Option" UI event.
/// </summary>
internal class DeleteOrganismScopeCultureTestOptionUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Organism Scope Culture Test Option" UI event.
    /// </summary>
    /// <returns>
    /// A string representation of the UI event configuration.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'deleteorganismscopeculturetestoptionuievent',
                        description: 'Delete Organism Scope Culture Test Option',
                        type: 'form',
                        action: 'deleteorganismscopeculturetestoptionform'
                    }";

        return newEvent;
    }
}
