using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditTableQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'edittablequery', 'TableName': 'List', 'Type': 'Single', 'ResultMapping': 'edittablequerymapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'Description', 'Type': 'string'}
                        ],
                        'Where' : [
                            {'Field': 'metaflistid', 'Comparison': 'equals', 'FieldToMatch': 'Id' }
                        ]
                    }";
        }
    }
}
