using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ResultExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'resultexists', 'TableName': 'ListItem', 'Type': 'Count',
                        'ParameterMapping': 'resultexistsmapper',
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
