using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteWorkflowEntryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteworkflowentry', 
                        Description: '@ConDelR@',
                        EventType : 'special', 
                        Topic : 'Configuration',
                        TableName: 'ListItem'
                    }";
        }
    }
}
