using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class KOHPrepTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'kohpreptestpage',
                            pageTitle: '@TesFun@',
                            text: '@TesEntL@.',
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
                                                { id: 'kohresultId', type: 'dropdown', label: '@TesTesA@', optionsName: 'kohresult', required: true},
                                                { id: 'kohFungalId', type: 'dropdown', label: '@TesFunC@', optionsName: 'fungus', required: false, parentList: 'kohresultId'},
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
