using arc.app.Common;

namespace arc.app.Config.Forms.Specification;

/// <summary>
/// Form configuration for deleting a document type list item.
/// </summary>
internal class DeleteDocumentFormConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var form = @"{
                        name: 'deletedocumentform',
                        viewTitle: 'Delete a document type.',
                        saveEvent: 'deletedocument',
                        suppressRecordView: true,
                        pages: ['deletedocumentpage']
                    }";

        return form;
    }
}
