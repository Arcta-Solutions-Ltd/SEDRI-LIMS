using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class PreferenceEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'preference', 
                        Description: '@UsePre@',
                        EventType : 'special', 
                        Topic : 'User', 
                        TableName: 'Users'
                    }";
        }
    }
}
