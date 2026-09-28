using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class CodingListItemExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'codinglistitemexists', 'TableName': 'ListItem', 'Type': 'Count',
                        'ParameterMapping': 'codinglistexistsmapper',
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
