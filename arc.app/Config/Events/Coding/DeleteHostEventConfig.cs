using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteHostEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deletehost', 
                        Description: '@BreDelC@',
                        EventType : 'deletedata', 
                        Topic : 'Breakpoints',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@BreAA@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'hostexistsinbreakpoint', message: '@BreCan@' }
                        ]
                    }";
        }
    }
}
