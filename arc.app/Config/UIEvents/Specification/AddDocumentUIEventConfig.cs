using arc.app.Common;

namespace arc.app.Config.UIEvents.Specification;

/// <summary>
/// UI event configuration for the add document type action.
/// </summary>
internal class AddDocumentUIEventConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var newEvent = @"{
                        name: 'adddocumentuievent',
                        description: 'Add Document Type',
                        type: 'form',
                        action: 'adddocumentform'
                    }";

        return newEvent;
    }
}
