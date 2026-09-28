using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditBreakpointEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                EventName: 'editBreakpoint', 
                Description: '@BreEdi@',
                EventType : 'special', 
                Topic : 'Breakpoints',
                ValidationRules: [
                    { field: 'AntibioticId', rule: 'required', message: '@BreAn@'},
                    { field: 'TestMethodId', rule: 'required', message: '@BreAB@'},
                    { field: 'HostId', rule: 'required', message: '@BreAA@'},
                    { field: 'SpecificationId', rule: 'required', message: '@BreAF@' },
                    { field: 'SpecialConsiderId', rule: 'required', message: '@BreAG@' }
                ]
            }";
        }
    }
}
