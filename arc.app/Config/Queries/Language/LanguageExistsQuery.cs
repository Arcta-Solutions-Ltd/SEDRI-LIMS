using arc.app.Common;

namespace arc.app.Config.Queries
{
    public class LanguageExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'languageexists', 'TableName': 'ListItem', 'Type': 'Count',
                        'ParameterMapping': 'languageexistsmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'Value', 'Comparison': 'equals' },
                            {'Field': 'ListId', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
