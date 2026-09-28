using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OrderTableQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'ordertablequery', 'TableName': 'ListItem', 'Type': 'Config'
                    }";
        }
    }
}
