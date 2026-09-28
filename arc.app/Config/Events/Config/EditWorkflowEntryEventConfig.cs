using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditWorkflowEntryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editworkflowentry', 
                        Description: '@ConEdiQ@',
                        EventType : 'special', 
                        Topic : 'Configuration',
                        TableName: 'ListItem',
                        ValidationRules: [
                            { field: 'EventField', rule: 'required', message: '@ConAne@'},
                            { field: 'EntryStates', rule: 'required', message: '@ConAneA@'}
                        ],
                    }";
        }
    }
}
