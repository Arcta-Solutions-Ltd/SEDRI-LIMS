using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class BiochemistryTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'biochemistrytestpage',
                            pageTitle: '@TesBio@',
                            text: '@TesEntM@.',
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
                                                { id: 'glucose', type: 'number', label: '@TesGlu@', required: true, Min: '0', Max: '100', MaxDPs: '1', Suffix: 'mM/L' },
                                                { id: 'protein', type: 'number', label: '@TesPro@', required: true, Min: '0', Max: '100', MaxDPs: '1', Suffix: 'g/L' },
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
