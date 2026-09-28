using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class GramStainTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'gramstaintestpage',
                            pageTitle: '@TesGra@',
                            text: '@TesEntC@.',
                            configureActions: 'add,edit',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'wbc', type: 'combobox', label: '@TesWbcB@', optionsName: 'seen'},
                                                { id: 'epicells', type: 'combobox', label: '@TesEpi@', optionsName: 'seen'},
                                                { id: 'organismgrid', type: 'fieldgrid', label: '@GenOrgB@', gridfields: [
                                                        { id: 'organism', gridtitle: 'organism', type: 'dropdown', optionsName: 'organism' },
                                                        { id: 'wbclist', gridtitle: 'wbclist', type: 'dropdown', optionsName: 'seen' }
                                                    ]
                                                },
                                                { id: 'printonreport', type: 'toggle', label: '@GenDis@', defaultValue: 'Yes', Configurable: 'No'}
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}
