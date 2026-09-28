using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EventPermissionEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'eventPermissions', 
                        Description: '@RolUpd@',
                        EventType : 'editdata', 
                        TableName : 'Role',
                        StringFields: 'EventPermission',
                        Topic: 'Role',
                        Mapping: 'eventPermissionsMapper'
                    }";
        }
    }
}
