using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteUserPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deleteuserpage',
                            pageTitle: '@UseDel@',
                            text: '@UseDelB@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'UserName', type: 'text', label: '@GenNam@'}
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
