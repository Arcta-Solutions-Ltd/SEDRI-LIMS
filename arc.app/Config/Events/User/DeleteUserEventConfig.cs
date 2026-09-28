using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteUserEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteUser', 
                        Description: '@UseDelA@',
                        EventType : 'special', 
                        Topic : 'User', 
                        TableName: 'Users'
                    }";
        }
    }
}
