using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddTestPatternEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addTestPattern', 
                        Description: '@TesAdd@',
                        EventType : 'specialadddata', 
                        Topic : 'TestPattern',
                        ValidationRules: [
                            { field: 'TestPatternName', rule: 'required', message: '@BreAD@'},
                            { field: 'hostid', rule: 'required', message: '@BreAA@'}
                        ]
                    }";
        }
    }
}
