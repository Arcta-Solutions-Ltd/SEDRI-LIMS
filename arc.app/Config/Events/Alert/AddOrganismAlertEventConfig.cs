using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddOrganismAlertEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addOrganismAlert', 
                        Description: '@AleAddA@',
                        EventType : 'specialadddata', 
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
