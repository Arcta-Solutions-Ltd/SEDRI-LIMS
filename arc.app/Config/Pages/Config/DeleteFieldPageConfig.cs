using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteFieldPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'deletefieldpage',
                            pageTitle: '@ConDelK@',
                            text: '@ConDelL@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'name', type: 'text', label: '@GenNam@' }
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
