using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the Manage Culture Attachments page.
/// Single upload field with MultiSelect for managing culture file attachments.
/// </summary>
internal class ManageCultureAttachmentsPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration.
    /// </summary>
    public string Get()
    {
        var page = @"{
            name: 'managecultureattachmentspage',
            pageTitle: '@GenAtt@',
            text: '@GenAtt@',
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

        return page;
    }
}
