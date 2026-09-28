using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class WrightsStainTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'wrightsstaintestpage',
                            pageTitle: '@TesWriA@',
                            text: '@TesEntK@.',
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
                                                { id: 'wrightsstainresultId', type: 'dropdown', label: '@TesTesA@', optionsName: 'wrightsstain', required: true},
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
