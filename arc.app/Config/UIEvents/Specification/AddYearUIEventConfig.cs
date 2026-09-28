using arc.app.Common;

namespace arc.app.Config.UIEvents.Specification;

/// <summary>
/// UI event configuration for the add publication year action.
/// </summary>
internal class AddYearUIEventConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var newEvent = @"{
                        name: 'addyearuievent',
                        description: 'Add Publication Year',
                        type: 'form',
                        action: 'addyearform'
                    }";

        return newEvent;
    }
}
