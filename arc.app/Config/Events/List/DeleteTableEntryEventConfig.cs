using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteTableEntryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteTableEntry', 
                        Description: '@TabDel@',
                        EventType : 'special', 
                        Topic : 'Lists', 
                        TableName: 'ListItem',
                        DataRules: [
                            { type: 'NoRecord', query: 'checkwhethertableentryisfixed', message: '@TabFix@' }
                        ]
                    }";
        }
    }
}
