using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class ChangePasswordPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'changepasswordpage',
                            pageTitle: '@UseCha@', 
                            text: '@UseChaA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'UserName', type: 'text', label: '@UseUseB@', required: true},
                                                { id: 'Password', type: 'password', label: '@UseNew@', required: true, Max: 100}
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
