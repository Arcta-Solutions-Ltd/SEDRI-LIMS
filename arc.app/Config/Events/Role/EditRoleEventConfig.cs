using arc.app.Common;

namespace arc.app.Config.Events
{ 
    internal class EditRoleEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'EditRole', 
                        Description: '@RolEdi@',
                        EventType : 'editdata', 
                        Topic : 'Role', 
                        TableName: 'Role',
                        StringFields: 'MoreData',
                        ValidationRules: [
                            { field: 'RoleName', rule: 'required', message: '@RolNam@'},
                            { field: 'RoleDescription', rule: 'required', message: '@RolDes@'},
                            { field: 'Enabled', rule: 'required', message: '@RolEna@'}
                        ]
                    }";
        }
    }
}
