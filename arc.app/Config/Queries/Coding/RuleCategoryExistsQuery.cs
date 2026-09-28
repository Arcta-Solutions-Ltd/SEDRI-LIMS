using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class RuleCategoryExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'rulecategoryexists', 'TableName': 'ListItem', 'Type': 'Count',
                        'ParameterMapping': 'rulecategoryexistsmapper',
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
