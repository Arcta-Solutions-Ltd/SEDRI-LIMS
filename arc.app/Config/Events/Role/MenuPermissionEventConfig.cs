using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class MenuPermissionEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'menuPermissions', 
                        Description: '@RolUpdA@',
                        EventType : 'editdata', 
                        TableName : 'Role',
                        Topic: 'Role',
                        StringFields: 'MenuPermission',
                        Mapping: 'menuPermissionsMapper'
                    }";
        }
    }
}
