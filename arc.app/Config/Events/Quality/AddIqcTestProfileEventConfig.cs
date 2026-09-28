using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddIqcTestProfileEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                EventName: 'addiqctestprofile', 
                Description: '@QuaAddProA@',
                EventType : 'special', 
                Topic: 'Quality',
                ValidationRules: [
                    { field: 'Name', rule: 'required', message: '@QuaAddProC@'},
                    { field: 'TestMethodId', rule: 'required', message: '@BreAB@'}
                ],
                DataRules: [
                    { type: 'NoRecord', query: 'duplicateiqctestprofilenamequery', message: '@QuaAddProD@' }
                ]
            }";
        }
    }
}
