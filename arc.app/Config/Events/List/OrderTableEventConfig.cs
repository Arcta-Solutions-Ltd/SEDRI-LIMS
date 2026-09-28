using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class OrderTableEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'OrderTable', 
                        Description: '@TabOrdA@',
                        EventType : 'special', 
                        Topic : 'Lists', 
                        TableName: 'ListItem'
                    }";
        }
    }
}
