using arc.app.Common;

namespace arc.app.Config.Pages.Specification;

/// <summary>
/// Page configuration for deleting a document type list item.
/// </summary>
internal class DeleteDocumentPageConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var page = @"{ 
                            name: 'deletedocumentpage',
                            pageTitle: '@SpfDelDoc@',
                            text: '@SpfDelDocA@.',
                            required: 'Name',
                            requiredRule: 'and',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Id', type: 'dropdown', label: '@GenNam@', required: true, placeholder: '@SpfDocPlaceholder@', optionsName: 'documenttype', dynamic: true, removeFixed: true }
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
