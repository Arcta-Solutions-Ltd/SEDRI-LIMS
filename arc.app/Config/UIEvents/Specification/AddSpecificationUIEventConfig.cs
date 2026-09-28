using arc.app.Common;

namespace arc.app.Config.UIEvents.Specification;

/// <summary>
/// UI event configuration for the Add Specification action.
/// </summary>
internal class AddSpecificationUIEventConfig : IDefinition
{
    public string Get()
    {
        var newEvent = @"{
                        name: 'addspecificationuievent',
                        description: 'Add Specification',
                        type: 'form',
                        action: 'addspecificationform'
                    }";

        return newEvent;
    }
}
