using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddUserDetailsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'useradddetails',
                            pageTitle: '@UseAddA@', 
                            text: '@UseAdd@.',
                            required: 'UserName,Email,Password,Roles',
                            requiredRule: 'and',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'UserName', type: 'singleline', label: '@UseUseB@', required: true, Max: 30},
                                                { id: 'FirstName', type: 'singleline', label: '@UseFir@', Max: 30},
                                                { id: 'LastName', type: 'singleline', label: '@UseLas@', Max: 30},
                                                { id: 'Email', type: 'singleline', label: '@UseEma@', required: true, Max: 60},
                                                { id: 'Roles', type: 'dropdown', label: '@RolRolC@', required: true, multiSelect: true, placeholder: 'Select Role', optionsName: 'RoleList', dynamic: true },
                                                { id: 'Password', type: 'password', label: '@UsePasA@', required: true, Max: 100},
                                                { id: 'Enabled', type: 'toggle', label: '@GenEna@', required: true, value: 'Yes', defaultValue: 'Yes' }
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

