using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal  class GramCultureTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'gramculturetestpage',
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
                                                { id: 'gcepicells', type: 'dropdown', label: '@TesEpi@', optionsName: 'seen', required: true},
                                                { id: 'gcorganismgrid', type: 'fieldgrid', label: '@GenOrgB@', gridfields: [
                                                        { id: 'gcorganism', type: 'dropdown', optionsName: 'organism' },
                                                        { id: 'gcwbclist', type: 'dropdown', optionsName: 'seen' }
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
