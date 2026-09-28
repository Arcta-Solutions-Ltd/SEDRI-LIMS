using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DuplicateListItemQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'duplicatelistitemquery', 'TableName': 'ListItem', 'Type': 'Count', 'ParameterMapping': 'duplicatelistitemmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '!=', 'FieldToMatch': 'id' },
                            {'Field': 'ListId', 'Comparison': 'equals' },
                            {'Field': 'Value', 'Comparison': 'equals' },
                            {'Field': 'Deleted', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
