using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Represents the configuration for the "Delete Storage Form".
/// </summary>
internal class DeleteStorageFormConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Delete Storage Form".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the "Delete Storage Form".
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'deletestorageform',
                        viewTitle: 'Delete an existing storage location.',
                        saveEvent: 'deletestorageevent',
                        initialQuery: 'editstoragequery',
                        suppressRecordView: true,
                        pages: [ 'deletestoragepage']
                    }";

        return form;
    }
}

