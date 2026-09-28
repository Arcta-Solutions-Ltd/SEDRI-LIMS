using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Represents the configuration for the "Add Storage Form".
/// </summary>
internal class AddStorageFormConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the "Add Storage Form".
    /// </summary>
    /// <returns>
    /// A JSON string representing the configuration for the "Add Storage Form".
    /// </returns>
    public string Get()
    {
        var form = @"{
                        name: 'addstorageform',
                        viewTitle: 'Add a new storage location.',
                        saveEvent: 'addstorageevent',
                        suppressRecordView: true,
                        pages: [ 'addstoragepage']
                    }";

        return form;
    }
}

