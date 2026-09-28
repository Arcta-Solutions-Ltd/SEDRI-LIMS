using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DuplicateListNameQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'duplicatelistnamequery', 'TableName': 'List', 'Type': 'Count',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'Name', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
