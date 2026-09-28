using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OrganismCodingExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'organismcodingexists', 'TableName': 'OrganismCoding', 'Type': 'Count',
                        'ParameterMapping': 'organismcodingmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'OrganismId', 'Comparison': 'equals' },
                            {'Field': 'CodingId', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
