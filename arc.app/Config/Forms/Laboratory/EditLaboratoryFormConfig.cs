using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Defines the configuration for the "Edit Laboratory" form.
/// </summary>
internal class EditLaboratoryFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the form configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the form structure and associated parameters.</returns>
    public string Get()
    {
        var form = @"{
                        name: 'editlaboratoryform',
                        title: 'Edit an existing laboratory.',
                        initialQuery: 'laboratorybyid',
                        saveEvent: 'editlaboratory',
                        suppressRecordView: true,
                        configurable: 'Yes',
                        pages: ['editlaboratorypage']
                    }";

        return form;
    }
}
