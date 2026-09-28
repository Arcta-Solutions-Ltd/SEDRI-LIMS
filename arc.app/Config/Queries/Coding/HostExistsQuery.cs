using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class HostExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'hostexists', 'TableName': 'ListItem', 'Type': 'Count',
                        'ParameterMapping': 'hostexistsmapper',
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
