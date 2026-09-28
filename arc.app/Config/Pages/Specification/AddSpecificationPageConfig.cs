using arc.app.Common;

namespace arc.app.Config.Pages.Specification;

/// <summary>
/// Page configuration for the Add Specification form.
/// </summary>
internal class AddSpecificationPageConfig : IDefinition
{
    public string Get()
    {
        var page = @"{ 
                            name: 'addspecificationpage',
                            pageTitle: '@SpfAdd@',
                            text: '@SpfAddA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'guidelinesid', type: 'combobox', optionsName: 'guidelines', label: '@GenGui@', width: 'wide', required: true, dynamic: true },
                                                { id: 'documentid', type: 'combobox', optionsName: 'documenttype', label: '@GenDoc@', width: 'wide', required: true, dynamic: true },
                                                { id: 'versionnumberid', type: 'combobox', optionsName: 'version', label: '@Ver@', width: 'wide', required: false, dynamic: true },
                                                { id: 'publicationyearid', type: 'combobox', optionsName: 'publicationyear', label: '@GenYeaB@', width: 'wide', required: false, dynamic: true, includeFixed: true }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";
        return page;
    }
}
