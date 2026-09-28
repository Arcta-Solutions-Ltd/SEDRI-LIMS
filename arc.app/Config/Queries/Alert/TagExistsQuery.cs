using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class TagExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'tagexists', 'TableName': 'ListItem', 'Type': 'Count',
                        'ParameterMapping': 'tagexistsmapper',
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
