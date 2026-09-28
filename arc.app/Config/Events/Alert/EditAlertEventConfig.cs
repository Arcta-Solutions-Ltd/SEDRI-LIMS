using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditAlertEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editAlert', 
                        Description: '@AleEdi@',
                        EventType : 'special', 
                        Topic : 'Alert', 
                        TableName: 'Alert',
                        ValidationRules: [
                            { field: 'AlertName', rule: 'required', message: '@AleAleA@'},
                            { field: 'AlertMessage', rule: 'required', message: '@AleAleB@'}
                        ]
                    }";
        }
    }
}
