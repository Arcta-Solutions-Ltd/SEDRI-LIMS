using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteRoleEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteRole', 
                        Description: '@RolDel@',
                        EventType : 'special', 
                        Topic : 'Role', 
                        TableName: 'Role',
                        DataRules: [
                            { type: 'NoRecord', query: 'usersinrolecountquery', message: '@RolThi@' }
                        ]
                    }";
        }
    }
}
