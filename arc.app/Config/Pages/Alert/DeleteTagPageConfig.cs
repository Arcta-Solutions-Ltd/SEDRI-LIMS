using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteTagPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deletetagpage',
                            pageTitle: '@AleDelB@',
                            text: '@AleDelC@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'text', label: '@GenTagA@' }
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
