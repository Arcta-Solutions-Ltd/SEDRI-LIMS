using arc.app.Common;

namespace arc.app.Config.UIEvents.Specification;

/// <summary>
/// UI event configuration for the delete publication year action.
/// </summary>
internal class DeleteYearUIEventConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var newEvent = @"{
                        name: 'deleteyearuievent',
                        description: 'Delete Publication Year',
                        type: 'form',
                        action: 'deleteyearform'
                    }";

        return newEvent;
    }
}
