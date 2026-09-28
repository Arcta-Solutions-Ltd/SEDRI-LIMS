using arc.app.Common;

namespace arc.app.Config.UIEvents.Specification;

/// <summary>
/// UI event configuration for the delete document type action.
/// </summary>
internal class DeleteDocumentUIEventConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var newEvent = @"{
                        name: 'deletedocumentuievent',
                        description: 'Delete Document Type',
                        type: 'form',
                        action: 'deletedocumentform'
                    }";

        return newEvent;
    }
}
