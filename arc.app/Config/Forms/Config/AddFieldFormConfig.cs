using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Provides the form configuration definition for adding a new field to a form.
/// This form is used to create and configure new fields within a page configuration.
/// </summary>
internal class AddFieldFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string representation of the add field form configuration.
    /// </summary>
    /// <returns>A JSON string containing the form configuration with the save event and page reference.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'addfieldform',
                        viewTitle: 'Add a new field.',
                        saveEvent: 'addfield',
                        suppressRecordView: true,
                        initialQuery: 'fieldparentlinkquery',
                        pages: [ 'addfieldpage']
                    }";

        return form;
    }
}
