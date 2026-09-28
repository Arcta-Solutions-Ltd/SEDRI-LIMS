using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteIqcResultEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                EventName: 'deleteiqcresult', 
                Description: '@QuaDelIqcTesRes@',
                EventType : 'deletedata', 
                Topic : 'Quality',
                TableName: 'iqcresults',
                ValidationRules: [
                    { field: 'Id', rule: 'required', message: '@QuaRemIdMis@'}
                ]
            }";
        }
    }
}
