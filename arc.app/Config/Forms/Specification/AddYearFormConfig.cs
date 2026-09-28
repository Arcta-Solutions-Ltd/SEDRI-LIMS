using arc.app.Common;

namespace arc.app.Config.Forms.Specification;

/// <summary>
/// Form configuration for adding a new publication year list item.
/// </summary>
internal class AddYearFormConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var form = @"{
                        name: 'addyearform',
                        viewTitle: 'Add a new publication year.',
                        saveEvent: 'addyear',
                        suppressRecordView: true,
                        pages: ['addyearpage']
                    }";

        return form;
    }
}
