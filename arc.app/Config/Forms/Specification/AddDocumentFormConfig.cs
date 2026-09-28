using arc.app.Common;

namespace arc.app.Config.Forms.Specification;

/// <summary>
/// Form configuration for adding a new document type list item.
/// </summary>
internal class AddDocumentFormConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var form = @"{
                        name: 'adddocumentform',
                        viewTitle: 'Add a new document type.',
                        saveEvent: 'adddocument',
                        suppressRecordView: true,
                        pages: ['adddocumentpage']
                    }";

        return form;
    }
}
