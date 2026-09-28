using arc.app.Common;

namespace arc.app.Config.Forms.Specification;

/// <summary>
/// Form configuration for deleting a version number list item.
/// </summary>
internal class DeleteVersionFormConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var form = @"{
                        name: 'deleteversionform',
                        viewTitle: 'Delete a version number.',
                        saveEvent: 'deleteversion',
                        suppressRecordView: true,
                        pages: ['deleteversionpage']
                    }";

        return form;
    }
}
