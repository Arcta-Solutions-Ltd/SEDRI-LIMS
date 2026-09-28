using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditIqcResultEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                EventName: 'editiqcresult', 
                Description: '@QuaEdiIqcRes@',
                EventType : 'special', 
                Topic : 'Quality',
                TableName: 'iqctests',
                ValidationRules: [
                    { field: 'value', rule: 'required', message: '@GenResA@' }
                ]
            }";
        }
    }
}
