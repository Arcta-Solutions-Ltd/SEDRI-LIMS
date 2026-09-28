using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddCultureTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addCultureTest',
                        Description: '@ConAddA@',
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
