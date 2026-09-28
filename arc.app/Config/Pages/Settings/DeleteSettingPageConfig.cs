using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteSettingPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'deletesettingpage',
                            pageTitle: '@SetDel@',
                            text: '@SetDelA@',
                            wide: false,
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
		                                { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'setting', type: 'text', label: '@GenSetA@' },
                                            ]
                                        },
                                        {
                                            key: 'fg2',
                                            fields: [
                                                { id: 'value', type: 'text', label: '@GenTex@', placeholder: '@SetAddAccTexC@' },
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
