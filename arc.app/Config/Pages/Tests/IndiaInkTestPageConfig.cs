using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class IndiaInkTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'indiainktestpage',
                            pageTitle: '@TesInd@',
                            text: '@TesEntD@.',
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
                                                { id: 'indiainkresult', type: 'dropdown', label: '@TesIndA@', optionsName: 'indiaink'}
                                            ]
                                        },
                                        { 
                                            key: 'fg2',
                                            rules:[{ effect: 'visible', field: 'indiainkresult', rule: '=', value: '256' }
                                            ],
                                            fields: [
                                                { id: 'positiveresult', type: 'dropdown', label: '@TesPos@', optionsName: 'indiainkresult'}
                                            ]
                                        },
                                        { 
                                            key: 'fg3',
                                            fields: [
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
