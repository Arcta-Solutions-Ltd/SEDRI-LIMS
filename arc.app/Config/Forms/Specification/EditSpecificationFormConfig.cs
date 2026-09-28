using arc.app.Common;

namespace arc.app.Config.Forms.Specification;

/// <summary>
/// Form configuration for editing an existing specification.
/// </summary>
internal class EditSpecificationFormConfig : IDefinition
{
    public string Get()
    {
        var form = @"{
                        name: 'editspecificationform',
                        viewTitle: 'Edit an existing specification.',
                        saveEvent: 'editspecification',
                        initialQuery: 'editspecificationquery',
                        suppressRecordView: true,
                        pages: ['editspecificationpage']
                    }";

        return form;
    }
}
