using arc.app.Common;

namespace arc.app.Config.Pages.Admission;

/// <summary>
/// Configuration for the Manage Admission Attachments page.
/// Single upload field with MultiSelect for managing admission file attachments.
/// </summary>
internal class ManageAdmissionAttachmentsPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'manageadmissionattachmentspage',
            pageTitle: '@GenAtts@',
            text: '@GenAtts@',
            columns: [
                {
                    key: 'col1',
                    fieldWidth: 'wide',
                    itemWidth: 'wide',
                    formGroups: [
                        {
                            key: 'fg1',
                            fields: [
                                { id: 'FileAttachmentIds', type: 'upload', label: '@GenAtts@', MultiSelect: true, Category: 'attachment' }
                            ]
                        }
                    ]
                }
            ]
        }";
    }
}
