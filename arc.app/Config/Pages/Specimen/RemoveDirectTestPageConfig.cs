using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class RemoveDirectTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'removedirecttestpage',
                            pageTitle: '@GenDel@',
                            text: '@TesDelE@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Title', type: 'text', label: '@GenTesC@' },
                                                { id: 'Status', type: 'text', label: '@GenStaA@' }
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
