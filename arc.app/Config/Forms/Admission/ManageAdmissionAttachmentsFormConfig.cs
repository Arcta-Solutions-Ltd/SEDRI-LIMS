using arc.app.Common;

namespace arc.app.Config.Forms.Admission;

/// <summary>
/// Configuration for the Manage Admission Attachments form.
/// </summary>
internal class ManageAdmissionAttachmentsFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the Manage Admission Attachments form.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'manageadmissionattachmentsform',
            viewTitle: '@GenAtts@',
            saveEvent: 'manageadmissionattachments',
            recordView: 'admissionrecordview',
            suppressRecordView: false,
            InitialQuery: 'manageadmissionattachmentsforminitialquery',
            pages: [ 'manageadmissionattachmentspage' ]
        }";
    }
}
