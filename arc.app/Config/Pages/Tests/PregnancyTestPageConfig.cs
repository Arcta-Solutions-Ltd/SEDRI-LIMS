using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class PregnancyTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'pregnancytestpage',
                            pageTitle: '@TesPre@',
                            text: '@TesEntG@.',
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
                                                { id: 'pregnancyId', type: 'dropdown', label: '@TesTesA@', optionsName: 'pregnancy', required: true},
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
