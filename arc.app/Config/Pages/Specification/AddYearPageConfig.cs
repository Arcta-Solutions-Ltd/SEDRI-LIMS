using arc.app.Common;

namespace arc.app.Config.Pages.Specification;

/// <summary>
/// Page configuration for adding a new publication year list item.
/// </summary>
internal class AddYearPageConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var page = @"{ 
                            name: 'addyearpage',
                            pageTitle: '@SpfAddYea@',
                            text: '@SpfAddYeaA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@SpfYeaPlaceholder@', Max: 100 }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}
