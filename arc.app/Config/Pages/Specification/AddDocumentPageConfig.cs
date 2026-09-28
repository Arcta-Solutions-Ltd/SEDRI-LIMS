using arc.app.Common;

namespace arc.app.Config.Pages.Specification;

/// <summary>
/// Page configuration for adding a new document type list item.
/// </summary>
internal class AddDocumentPageConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var page = @"{ 
                            name: 'adddocumentpage',
                            pageTitle: '@SpfAddDoc@',
                            text: '@SpfAddDocA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@SpfDocPlaceholder@', Max: 100 }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}
