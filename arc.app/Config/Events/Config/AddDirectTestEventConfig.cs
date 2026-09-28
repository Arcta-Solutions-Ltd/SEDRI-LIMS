using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddDirectTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addDirectTest',
                        Description: '@ConAddB@',
                        EventType : 'specialadddata',
                        Topic : 'Configuration',
                        ValidationRules: [
                            { field: 'TestToCloneId', rule: 'required', message: '@ConYou@'},
                            { field: 'Title', rule: 'required', message: '@ConTit@'},
                            { field: 'Description', rule: 'required', message: '@ConAde@'}
                        ]
                    }";
        }
    }
}
