using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the Manage Specimen Attachments page.
/// Single upload field with MultiSelect for managing specimen file attachments.
/// </summary>
internal class ManageSpecimenAttachmentsPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page configuration.
    /// </summary>
    public string Get()
    {
        var page = @"{
            name: 'managespecimenattachmentspage',
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

        return page;
    }
}
