using arc.app.Common;

namespace arc.app.Config.Forms.Specification;

/// <summary>
/// Form configuration for adding a new version number list item.
/// </summary>
internal class AddVersionFormConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var form = @"{
                        name: 'addversionform',
                        viewTitle: 'Add a new version number.',
                        saveEvent: 'addversion',
                        suppressRecordView: true,
                        pages: ['addversionpage']
                    }";

        return form;
    }
}
