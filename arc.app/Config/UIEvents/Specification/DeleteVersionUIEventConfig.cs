using arc.app.Common;

namespace arc.app.Config.UIEvents.Specification;

/// <summary>
/// UI event configuration for the delete version number action.
/// </summary>
internal class DeleteVersionUIEventConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var newEvent = @"{
                        name: 'deleteversionuievent',
                        description: 'Delete Version Number',
                        type: 'form',
                        action: 'deleteversionform'
                    }";

        return newEvent;
    }
}
