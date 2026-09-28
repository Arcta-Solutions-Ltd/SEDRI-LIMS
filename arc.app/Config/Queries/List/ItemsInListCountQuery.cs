using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ItemsInListCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'itemsinlistcountquery', 'TableName': 'ListItem', 'Type': 'Count',
                        'Fields': [
                            {'Name': 'ListId', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'ListId', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
