using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Manage Patient Attachments" form.
/// Allows adding and removing file attachments for a patient from the patient record view.
/// </summary>
internal class ManagePatientAttachmentsFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the Manage Patient Attachments form.
    /// </summary>
    public string Get()
    {
        var form = @"{
            name: 'managepatientattachmentsform',
            viewTitle: '@GenAtts@',
            saveEvent: 'managepatientattachments',
            recordView: 'patientrecordview',
            suppressRecordView: false,
            InitialQuery: 'managepatientattachmentsforminitialquery',
            pages: [ 'managepatientattachmentspage' ]
        }";

        return form;
    }
}
