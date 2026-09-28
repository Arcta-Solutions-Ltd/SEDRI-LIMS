using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddTableEntryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addTableEntry', 
                        Description: '@TabAdd@',
                        EventType : 'specialadddata', 
                        Topic : 'Lists', 
                        TableName: 'ListItem',
                        Mapping: 'addtableentrymapper',
                        DataRules: [
                            { type: 'NoRecord', query: 'duplicatelistitemquery', message: '@TabThiA@' }
                        ]
                    }";
        }
    }
}
