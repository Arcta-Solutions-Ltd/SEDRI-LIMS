using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditResultEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editresult', 
                        Description: '@BreEdiF@',
                        EventType : 'editdata', 
                        Topic : 'Breakpoints',
                        Mapping: 'editresultmapper',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'ResultId', rule: 'required', message: '@BreTheB@'},
                            { field: 'Name', rule: 'required', message: '@BreAC@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'resultexists', message: '@BreThiB@' }
                        ]
                    }";
        }
    }
}
