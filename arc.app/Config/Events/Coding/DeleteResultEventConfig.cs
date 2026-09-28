using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteResultEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteresult', 
                        Description: '@BreDelE@',
                        EventType : 'deletedata', 
                        Topic : 'Breakpoints',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@BreAC@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'resultexistsinbreakpoint', message: '@BreCanB@' }
                        ]
                    }";
        }
    }
}
