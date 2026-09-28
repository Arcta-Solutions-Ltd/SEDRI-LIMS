using arc.app.Common;

namespace arc.app.Config.Pages.Request;

/// <summary>
/// Configuration for the Manage Request Attachments page.
/// Single upload field with MultiSelect for managing request file attachments.
/// </summary>
internal class ManageRequestAttachmentsPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'managerequestattachmentspage',
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
