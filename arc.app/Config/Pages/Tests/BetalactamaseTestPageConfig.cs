using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class BetalactamaseTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'betalactamasetestpage',
                            pageTitle: '@TesBet@',
                            text: '@TesEntS@.',
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
                                                { id: 'betalactamaseresultId', type: 'combobox', label: '@TesBetA@', optionsName: 'betalactamase', required: true},
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
