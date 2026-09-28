using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class MyPasswordPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'mypasswordpage',
                            pageTitle: '@UseCha@', 
                            text: '@UseChaA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Password', type: 'password', label: '@UseNew@', required: true}
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}
