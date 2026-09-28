using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SourceListItemExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'sourcelistitemexists', 'TableName': 'ListItem', 'Type': 'Count',
                        'ParameterMapping': 'sourcelistexistsmapper',
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
