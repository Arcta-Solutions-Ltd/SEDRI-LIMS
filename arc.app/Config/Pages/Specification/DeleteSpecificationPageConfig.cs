using arc.app.Common;

namespace arc.app.Config.Pages.Specification;

/// <summary>
/// Page configuration for the Delete Specification form. Displays specification details
/// in read-only fields with a Delete button to confirm removal.
/// </summary>
internal class DeleteSpecificationPageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON page definition for the delete specification form, including
    /// field layout and the Delete button configuration.
    /// </summary>
    /// <returns>JSON string defining the page structure.</returns>
    public string Get()
    {
        var page = @"{ 
                            name: 'deletespecificationpage',
                            pageTitle: '@SpfDel@',
                            text: '@SpfDelA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'guidelines', type: 'text', label: '@GenGui@', width: 'medium' },
                                                { id: 'document', type: 'text', label: '@GenDoc@', width: 'medium' },
                                                { id: 'versionnumber', type: 'text', label: '@Ver@', width: 'medium' },
                                                { id: 'publicationyear', type: 'text', label: '@GenYeaB@', width: 'medium' }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenDelC@' }
                        }";
        return page;
    }
}
