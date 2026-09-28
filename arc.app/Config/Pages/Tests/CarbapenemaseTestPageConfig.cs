using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class CarbapenemaseTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'carbapenemasetestpage',
                            pageTitle: '@TesCarB@',
                            text: '@TesEntT@.',
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
                                                { id: 'carbapenemaseresultId', type: 'combobox', label: '@TesCarC@', optionsName: 'carbapenemase', required: true},
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
