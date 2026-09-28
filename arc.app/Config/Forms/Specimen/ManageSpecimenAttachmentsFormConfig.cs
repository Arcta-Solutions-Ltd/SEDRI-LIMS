using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Configuration for the "Manage Specimen Attachments" form.
/// Allows adding and removing file attachments for a specimen from the specimen record view.
/// </summary>
internal class ManageSpecimenAttachmentsFormConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the Manage Specimen Attachments form.
    /// </summary>
    public string Get()
    {
        var form = @"{
            name: 'managespecimenattachmentsform',
            viewTitle: '@GenAtts@',
            saveEvent: 'managespecimenattachments',
            recordView: 'specimenrecordview',
            suppressRecordView: false,
            InitialQuery: 'managespecimenattachmentsforminitialquery',
            pages: [ 'managespecimenattachmentspage' ]
        }";

        return form;
    }
}
