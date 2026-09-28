using arc.app.Common;

namespace arc.app.Config.Forms.Specification;

/// <summary>
/// Form configuration for deleting a publication year list item.
/// </summary>
internal class DeleteYearFormConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var form = @"{
                        name: 'deleteyearform',
                        viewTitle: 'Delete a publication year.',
                        saveEvent: 'deleteyear',
                        suppressRecordView: true,
                        pages: ['deleteyearpage']
                    }";

        return form;
    }
}
