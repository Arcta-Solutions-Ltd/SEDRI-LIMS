using arc.app.Common;

namespace arc.app.Config.UIEvents.Specification;

/// <summary>
/// UI event configuration for the Edit Specification action.
/// </summary>
internal class EditSpecificationUIEventConfig : IDefinition
{
    public string Get()
    {
        var newEvent = @"{
                        name: 'editspecificationuievent',
                        description: 'Edit Specification',
                        type: 'form',
                        action: 'editspecificationform'
                    }";

        return newEvent;
    }
}
