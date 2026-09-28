using arc.app.Common;

namespace arc.app.Config.Forms.Specification;

/// <summary>
/// Form configuration for adding a new specification.
/// </summary>
internal class AddSpecificationFormConfig : IDefinition
{
    public string Get()
    {
        var form = @"{
                        name: 'addspecificationform',
                        viewTitle: 'Add a new specification.',
                        saveEvent: 'addspecification',
                        suppressRecordView: true,
                        pages: ['addspecificationpage']
                    }";

        return form;
    }
}
