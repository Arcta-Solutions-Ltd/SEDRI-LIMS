using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteTableEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteTable', 
                        Description: '@TabDelD@',
                        EventType : 'special', 
                        Topic : 'Lists', 
                        TableName: 'List'
                    }";           
        }
    }
}
