using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class RemoveCultureTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'removeculturetestpage',
                            pageTitle: '@ConDelH@',
                            text: '@ConDelI@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'name', type: 'text', label: '@GenTesC@' }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenDelC@' }
                        }";

            return page;
        }
    }
}
