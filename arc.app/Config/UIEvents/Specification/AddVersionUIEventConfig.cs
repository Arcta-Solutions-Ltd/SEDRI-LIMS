using arc.app.Common;

namespace arc.app.Config.UIEvents.Specification;

/// <summary>
/// UI event configuration for the add version number action.
/// </summary>
internal class AddVersionUIEventConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var newEvent = @"{
                        name: 'addversionuievent',
                        description: 'Add Version Number',
                        type: 'form',
                        action: 'addversionform'
                    }";

        return newEvent;
    }
}
