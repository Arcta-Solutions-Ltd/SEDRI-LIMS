using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddAccessionNumberTextEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addaccessionnumbertext', 
                        Description: '@SetAddAccTex@',
                        EventType : 'special', 
                        Topic : 'Settings', 
                        TableName: 'Configs',
                        ValidationRules: [ { field: 'textvalue', rule: 'required', message: '@SetTexNulErr@'} ]
                    }";
        }
    }
}
