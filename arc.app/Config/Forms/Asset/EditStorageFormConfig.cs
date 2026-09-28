using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Represents the configuration for the "Edit Storage Form".
/// </summary>
internal class EditStorageFormConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Edit Storage Form".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the "Edit Storage Form".
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'editstorageform',
                        viewTitle: 'Edit an existing storage location.',
                        saveEvent: 'editstorageevent',
                        initialQuery: 'editstoragequery',
                        suppressRecordView: true,
                        pages: [ 'editstoragepage']
                    }";

        return form;
    }
}

