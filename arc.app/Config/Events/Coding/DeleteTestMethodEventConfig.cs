using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteTestMethodEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deletetestmethod', 
                        Description: '@BreDelB@',
                        EventType : 'deletedata', 
                        Topic : 'Breakpoints',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@BreAB@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'testmethodexistsinbreakpoint', message: '@BreCanA@' }
                        ]
                    }";
        }
    }
}
