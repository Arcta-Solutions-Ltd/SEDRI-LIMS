using arc.app.Common;

namespace arc.app.Config.Forms.Request;

/// <summary>
/// Configuration for the Manage Request Attachments form.
/// </summary>
internal class ManageRequestAttachmentsFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the Manage Request Attachments form.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'managerequestattachmentsform',
            viewTitle: '@GenAtts@',
            saveEvent: 'managerequestattachments',
            recordView: 'requestrecordview',
            suppressRecordView: false,
            InitialQuery: 'managerequestattachmentsforminitialquery',
            pages: [ 'managerequestattachmentspage' ]
        }";
    }
}
