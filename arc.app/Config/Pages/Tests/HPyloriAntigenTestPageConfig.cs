using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class HPyloriAntigenTestPageConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                            name: 'hpyloriantigentestpage',
                            pageTitle: '@TesHpy@',
                            text: '@TesEntI@.',
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
                                                { id: 'antResultId', type: 'dropdown', label: '@TesTesA@', optionsName: 'hpyloriantigen', required: true},
                                                { id: 'printonreport', type: 'toggle', label: '@GenDis@', defaultValue: 'Yes', Configurable: 'No'}
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";
        }
    }
}
