using arc.app.Common;

namespace arc.app.Config.Pages.Specification;

/// <summary>
/// Page configuration for deleting a publication year list item.
/// </summary>
internal class DeleteYearPageConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var page = @"{ 
                            name: 'deleteyearpage',
                            pageTitle: '@SpfDelYea@',
                            text: '@SpfDelYeaA@.',
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
                                                { id: 'Id', type: 'dropdown', label: '@GenNam@', required: true, placeholder: '@SpfYeaPlaceholder@', optionsName: 'publicationyear', dynamic: true, removeFixed: true }
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
