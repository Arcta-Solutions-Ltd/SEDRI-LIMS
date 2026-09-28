using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteStateEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deletestate', 
                        Description: '@ConDelQ@',
                        EventType : 'deletedata', 
                        Mapping: 'editstatemapper',
                        Topic : 'Configuration',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@ConThe@'}
                        ],
                        DataRules: [
                            { type: 'NoRecord', query: 'stateexistsinqueuequery', message: '@ConCan@' }
                        ]
                    }";
        }
    }
}
