using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class ExportConfigurationPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'exportconfigurationpage',
                            pageTitle: '@ConExpB@',
                            crafted: true,
                            text: '@ConExpC@',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Configuration', type: 'toggle', label: '@GenCon@', required: false, defaultValue: 'Yes' },
                                                { id: 'ListItems', type: 'toggle', label: '@GenLis@', required: false, defaultValue: 'Yes' },
                                                { id: 'Organisms', type: 'toggle', label: '@GenOrgB@', required: false, defaultValue: 'Yes' },
                                                { id: 'Language', type: 'toggle', label: '@GenLan@', required: false, defaultValue: 'Yes' }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { 
                                            show: false
                                        },
                        }";

            return page;
        }
    }
}
