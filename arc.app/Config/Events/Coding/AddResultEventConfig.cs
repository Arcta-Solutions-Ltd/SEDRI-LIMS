using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddResultEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addresult', 
                        Description: '@BreAddF@',
                        EventType : 'adddata', 
                        Topic : 'Breakpoints',
                        Mapping: 'addresultmapper',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@BreAC@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'resultexists', message: '@BreThiB@' }
                        ]
                    }";
        }
    }
}
