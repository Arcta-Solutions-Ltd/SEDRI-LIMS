using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class JEVSerologyTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'jevserologytestpage',
                            pageTitle: '@TesJevA@',
                            text: '@TesEntJ@.',
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
                                                { id: 'jevserologyResultId', type: 'dropdown', label: '@TesTesA@', optionsName: 'jevserology', required: true},
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
