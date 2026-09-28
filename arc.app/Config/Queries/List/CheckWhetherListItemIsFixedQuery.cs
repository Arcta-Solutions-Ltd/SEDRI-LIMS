using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class CheckWhetherListItemIsFixedQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'checkwhetherlistitemisfixed', 'TableName': 'ListItem', 'Type': 'Count',
                        'ParameterMapping': 'checkwhetherlistitemisfixedmapper',
                         'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': 'equals' },
                            {'Field': 'Fixed', 'Comparison': 'equals' },
                            {'Field': 'ListId', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
