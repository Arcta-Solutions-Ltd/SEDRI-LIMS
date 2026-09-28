using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditTableEntryEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editTableEntry', 
                        Description: '@TabEdi@',
                        EventType : 'special', 
                        Topic : 'Lists', 
                        TableName: 'ListItem',
                        DataRules: [
                            { type: 'NoRecord', query: 'duplicatelistitemquery', message: '@TabThiA@' }
                        ]
                    }";
        }
    }
}
