using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditStateEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editstate', 
                        Description: '@ConEdiP@',
                        EventType : 'editdata', 
                        Topic : 'Configuration',
                        Mapping: 'editstatemapper',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'StateId', rule: 'required', message: '@ConThe@'},
                            { field: 'Name', rule: 'required', message: '@ConAC@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'stateexistsquery', message: '@ConThi@' }
                        ]
                    }";
        }
    }
}
