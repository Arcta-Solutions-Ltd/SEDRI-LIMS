using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AlertDetailsSecondPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'alertdetailssecondpage',
                            pageTitle: '@AleCri@',
                            text: '@AleCriA@.',
                            wider: true,
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            rules:[{ effect: 'visible', field: 'OrganismConfigured', rule: '!=', value: 'No' }],
                                            fields: [
                                                { id: 'space', type: 'space' },
                                                { id: 'DoesExist', type: 'toggle', label: '@AleDoe@'},
                                                { id: 'sep', type: 'separator' }
                                            ]
                                        },
                                        { 
                                            key: 'fg2',
                                            rules:[{ effect: 'visible', field: 'DoesExist', rule: '!=', value: 'Yes' },
                                                   { effect: 'visible', field: 'OrganismConfigured', rule: '!=', value: 'No' }
                                            ],
                                            fields: [
                                                { id: 'SusceptibilityGrid', type: 'fieldgrid', label: '@AleSus@', gridfields: [
                                                        { id: 'AntibioticId', type: 'dropdown', optionsName: 'antibioticlist', multiselect: true, placeholder: '@GenAnt@' },
                                                        { id: 'SusceptibilityId', type: 'dropdown', optionsName: 'testresult', width: 'medium', placeholder: '@GenSus@' }
                                                    ]
                                                },
                                                { id: 'SusceptibilityAndOr', type: 'dropdown', optionsName: 'andor', label: '@AleSusA@', width: 'narrow'  },
                                                { id: 'space', type: 'space' },
                                                { id: 'sep', type: 'separator' }
                                            ]
                                        },
                                        {
                                            key: 'fg3',
                                            rules:[{ effect: 'visible', field: 'DoesExist', rule: '!=', value: 'Yes' }],
                                            fields: [
                                                { id: 'TestGrid', type: 'crafted', label: '@AleTes@', gridfields: [
                                                        { id: 'Test', type: 'dropdown', optionsName: 'testconfiglist'  },
                                                        { id: 'Field', type: 'dropdown', optionsName: 'fieldlist', dynamic: true },
                                                        { id: 'Comparison', type: 'dropdown', optionsName: 'comparison', width: 'small'  },
                                                        { id: 'StringValue', type: 'singleline', width: 'medium', Max: 20 },
                                                        { id: 'NumberValue', type: 'number', width: 'medium' },
                                                        { id: 'ListValue', type: 'dropdown', width: 'medium' }
                                                    ]
                                                },
                                                { id: 'TestAndOr', type: 'dropdown', optionsName: 'andor', label: '@AleTesA@', width: 'narrow'  }
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
