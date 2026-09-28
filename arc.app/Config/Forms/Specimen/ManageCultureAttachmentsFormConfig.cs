using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Manage Culture Attachments" form.
/// Allows adding and removing file attachments for a culture from the isolate record view.
/// </summary>
internal class ManageCultureAttachmentsFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the Manage Culture Attachments form.
    /// </summary>
    public string Get()
    {
        var form = @"{
            name: 'managecultureattachmentsform',
            viewTitle: '@GenAtts@',
            saveEvent: 'managecultureattachments',
            recordView: 'cultures',
            suppressRecordView: false,
            InitialQuery: 'managecultureattachmentsforminitialquery',
            pages: [ 'managecultureattachmentspage' ]
        }";

        return form;
    }
}
