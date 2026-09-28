using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class CatalaseTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'catalasetestpage',
                            pageTitle: '@TesCat@',
                            text: '@TesEntW@.',
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
                                                { id: 'catalaseresultId', type: 'combobox', label: '@TesCatA@', optionsName: 'catalase', required: true},
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
