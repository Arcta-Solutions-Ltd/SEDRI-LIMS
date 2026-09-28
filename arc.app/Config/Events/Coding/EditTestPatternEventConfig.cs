using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditTestPatternEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editTestPattern', 
                        Description: '@TesEdiD@',
                        EventType : 'special', 
                        Topic : 'TestPattern',
                        ValidationRules: [
                            { field: 'TestPatternName', rule: 'required', message: '@BreAD@'},
                            { field: 'hostid', rule: 'required', message: '@BreAA@'}
                        ]
                    }";
        }
    }
}
