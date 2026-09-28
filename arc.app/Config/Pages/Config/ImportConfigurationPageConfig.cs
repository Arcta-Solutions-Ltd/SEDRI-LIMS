using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class ImportConfigurationPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'importconfigurationpage',
                            pageTitle: '@ConImpB@',
                            crafted: false,
                            text: '@ConImpC@',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'upload', type: 'upload', label: '@ConConC@' }
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
