using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'deletetestpage',
                            pageTitle: '@ConDelH@',
                            text: '@ConDelI@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'title', type: 'text', label: '@GenTit@' }
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
