using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class TestMethodExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'testmethodexists', 'TableName': 'ListItem', 'Type': 'Count',
                        'ParameterMapping': 'testmethodexistsmapper',
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
