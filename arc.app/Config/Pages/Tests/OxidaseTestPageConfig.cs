using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class OxidaseTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'oxidasetestpage',
                            pageTitle: '@TesOxi@',
                            text: '@TesEntV@.',
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
                                                { id: 'oxidaseresultId', type: 'combobox', label: '@TesOxiA@', optionsName: 'oxidase', required: true},
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
