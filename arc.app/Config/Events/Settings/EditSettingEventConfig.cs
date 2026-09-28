using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditSettingEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editsetting', 
                        Description: '@EdiSet@',
                        EventType : 'special', 
                        Topic : 'Settings', 
                        TableName: 'Configs'
                    }";
        }
    }
}
