using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteAlertPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deletealertpage',
                            pageTitle: '@AleDel@',
                            text: '@AleDelA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AlertName', type: 'text', label: '@AleAle@'},
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
