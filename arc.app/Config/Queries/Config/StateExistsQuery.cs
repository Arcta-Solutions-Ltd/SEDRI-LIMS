using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class StateExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'stateexistsquery', 'TableName': 'ListItem', 'Type': 'Count',
                        'ParameterMapping': 'stateexistsmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'ListId', 'Comparison': 'equals' },
                            {'Field': 'Value', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
