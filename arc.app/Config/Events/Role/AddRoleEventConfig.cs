using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddRoleEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addRole', 
                        Description: '@RolAddA@',
                        EventType : 'adddata', 
                        Topic : 'Role', 
                        TableName: 'Role',
                        StringFields: 'MenuPermission, EventPermission',
                        Mapping: 'addRoleMapper',
                        ValidationRules: [
                            { field: 'RoleName', rule: 'required', message: '@RolNam@'},
                            { field: 'RoleDescription', rule: 'required', message: '@RolDes@'},
                            { field: 'Enabled', rule: 'required', message: '@RolEna@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'rolenameexists', message: '@RolCan@' }
                        ]
                    }";
        }
    }
}
