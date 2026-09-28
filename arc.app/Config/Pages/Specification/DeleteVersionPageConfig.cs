using arc.app.Common;

namespace arc.app.Config.Pages.Specification;

/// <summary>
/// Page configuration for deleting a version number list item.
/// </summary>
internal class DeleteVersionPageConfig : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        var page = @"{ 
                            name: 'deleteversionpage',
                            pageTitle: '@SpfDelVer@',
                            text: '@SpfDelVerA@.',
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
                                                { id: 'Id', type: 'dropdown', label: '@GenNam@', required: true, placeholder: '@SpfVerPlaceholder@', optionsName: 'version', dynamic: true, removeFixed: true }
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
