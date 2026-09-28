using arc.app.Common;

namespace arc.app.Config.Pages.Specification;

/// <summary>
/// Page configuration for the Edit Specification form.
/// </summary>
internal class EditSpecificationPageConfig : IDefinition
{
    public string Get()
    {
        var page = @"{ 
                            name: 'editspecificationpage',
                            pageTitle: '@SpfEdi@',
                            text: '@SpfEdiA@.',
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
                                                { id: 'publicationyearid', type: 'combobox', optionsName: 'publicationyear', label: '@GenYeaB@', width: 'wide', required: false, dynamic: true }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";
        return page;
    }
}
