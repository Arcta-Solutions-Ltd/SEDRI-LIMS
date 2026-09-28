using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteSectionPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'deletesectionpage',
                            pageTitle: '@ConDelO@',
                            text: '@ConDelP@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'name', type: 'text', label: '@GenNam@', required: true }
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
