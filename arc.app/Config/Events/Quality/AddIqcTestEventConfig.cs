using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddIqcTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                EventName: 'addiqctest', 
                Description: '@QuaAddA@',
                EventType : 'special', 
                Topic : 'Quality', 
                TableName: 'iqctests',
                ValidationRules: [
                    { field: 'TestProfileId', rule: 'required', message: '@QuaIqcTesProB@'}
                ]
            }";
        }
    }
}
