using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteSettingEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deletesetting', 
                        Description: '@SetDel@',
                        EventType : 'special', 
                        Topic : 'Settings', 
                        TableName: 'Configs'
                    }";
        }
    }
}
