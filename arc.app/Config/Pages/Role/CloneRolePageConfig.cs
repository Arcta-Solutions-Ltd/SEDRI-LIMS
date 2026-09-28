using arc.app.Common;

namespace arc.app.Config.Pages.Role
{
    internal class CloneRolePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'clonerolepage',
                            pageTitle: '@RolCloA@',
                            text: '@RolCloB@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            separator: '@RolRolD@',
                                            fields: [
                                                { id: 'RoleName', type: 'text', label: '@GenNam@'},
                                                { id: 'RoleDescription', type: 'text', label: '@GenDes@'},
                                             ]
                                        },
                                        { 
                                            key: 'fg2',
                                            separator: '@RolNew@',
                                            fields: [
                                                { id: 'NewRoleName', type: 'singleline', label: '@GenNam@', required: true, Max: 30},
                                                { id: 'NewRoleDescription', type: 'singleline', label: '@GenDes@', required: true, Max: 100},
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
