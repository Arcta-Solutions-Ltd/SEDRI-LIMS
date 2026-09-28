using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class CloneRoleEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'cloneRole', 
                        Description: '@RolClo@',
                        EventType : 'special', 
                        Topic : 'Role', 
                        TableName: 'Role',
                        ValidationRules: [
                            { field: 'NewRoleName', rule: 'required', message: '@RolNam@'},
                            { field: 'NewRoleDescription', rule: 'required', message: '@RolDes@'},
                            { field: 'Id', rule: 'required', message: '@GenId@'}
                        ]
                    }";
        }

    }
}
