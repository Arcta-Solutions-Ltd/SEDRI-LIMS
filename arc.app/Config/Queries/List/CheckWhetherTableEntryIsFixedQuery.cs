using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class CheckWhetherTableEntryIsFixedQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'checkwhethertableentryisfixed', 'TableName': 'ListItem', 'Type': 'Count',
                        'ParameterMapping': 'checkwhethertableentryisfixedmapper',
                         'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': 'equals' },
                            {'Field': 'Fixed', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
