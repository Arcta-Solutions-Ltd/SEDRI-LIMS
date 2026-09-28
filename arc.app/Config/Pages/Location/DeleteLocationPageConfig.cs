using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteLocationPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deletelocationpage',
                            pageTitle: '@LocDel@',
                            text: '@LocDelA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'text', label: '@GenLoc@'}
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
