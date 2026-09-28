using arc.app.Common;

namespace arc.app.Config.Pages.Specification;

/// <summary>
/// Page configuration for adding a new version number list item.
/// </summary>
internal class AddVersionPageConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var page = @"{ 
                            name: 'addversionpage',
                            pageTitle: '@SpfAddVer@',
                            text: '@SpfAddVerA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@SpfVerPlaceholder@', Max: 100 }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}
