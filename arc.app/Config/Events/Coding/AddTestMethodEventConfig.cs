using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddTestMethodEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addtestmethod', 
                        Description: '@BreAddB@',
                        EventType : 'adddata', 
                        Topic : 'Breakpoints',
                        Mapping: 'addtestmethodmapper',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@CodA@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'testmethodexists', message: '@BreThi@' }
                        ]
                    }";
        }
    }
}
