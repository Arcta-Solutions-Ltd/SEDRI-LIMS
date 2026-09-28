using arc.app.Common;

namespace arc.app.Config.Events
{
    public class AddStateEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addstate', 
                        Description: '@ConAddP@',
                        EventType : 'adddata', 
                        Topic : 'Configuration',
                        Mapping: 'addstatemapper',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@ConAC@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'stateexistsquery', message: '@ConThi@' }
                        ]
                    }";
        }
    }
}
