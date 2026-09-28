using arc.app.Common;

namespace arc.app.Config.Forms.Specification;

/// <summary>
/// Form configuration for deleting a specification.
/// </summary>
internal class DeleteSpecificationFormConfig : IDefinition
{
    public string Get()
    {
        var form = @"{
                        name: 'deletespecificationform',
                        viewTitle: 'Delete an existing specification.',
                        saveEvent: 'deletespecification',
                        initialQuery: 'deletespecificationquery',
                        suppressRecordView: true,
                        pages: ['deletespecificationpage']
                    }";

        return form;
    }
}
