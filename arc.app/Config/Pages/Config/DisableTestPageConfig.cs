using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DisableTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'disabletestpage',
                            pageTitle: '@ConDisB@',
                            text: '@ConDisC@',
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
                            nextButton: { show: true, buttonText: '@GenDisB@' }
                        }";

            return page;
        }
    }
}
