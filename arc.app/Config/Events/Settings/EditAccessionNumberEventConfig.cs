using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditAccessionNumberEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editaccessionnumber', 
                        Description: '@SetEdi@',
                        EventType : 'special', 
                        Topic : 'Settings', 
                        TableName: 'Configs'
                    }";
        }
    }
}
