using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteIqcTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                EventName: 'deleteiqctest', 
                Description: '@QuaDelIqcTes@',
                EventType : 'deletedata', 
                Topic : 'Quality',
                TableName: 'iqctests',
                ValidationRules: [
                    { field: 'Id', rule: 'required', message: '@QuaRemIdMisA@'}
                ],
            }";
        }
    }
}
