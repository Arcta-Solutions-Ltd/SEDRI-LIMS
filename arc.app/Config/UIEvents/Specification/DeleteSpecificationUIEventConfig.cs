using arc.app.Common;

namespace arc.app.Config.UIEvents.Specification;

/// <summary>
/// UI event configuration for the Delete Specification action.
/// </summary>
internal class DeleteSpecificationUIEventConfig : IDefinition
{
    public string Get()
    {
        var newEvent = @"{
                        name: 'deletespecificationuievent',
                        description: 'Delete Specification',
                        type: 'form',
                        action: 'deletespecificationform'
                    }";

        return newEvent;
    }
}
