using arc.app.Common;

namespace arc.app.Config.Pages.Role
{
    public class DeleteRolePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deleterolepage',
                            pageTitle: '@RolDelA@',
                            text: '@RolRem@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'RoleName', type: 'text', label: '@GenNam@'},
                                                { id: 'RoleDescription', type: 'text', label: '@GenDes@'},
                                                { id: 'Enabled', type: 'text', label: '@GenEna@', value: 'Yes'}
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
