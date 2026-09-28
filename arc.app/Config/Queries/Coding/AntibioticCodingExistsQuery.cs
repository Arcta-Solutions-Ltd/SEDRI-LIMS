using arc.app.Common;

namespace arc.app.Config.Queries.Coding
{
    internal class AntibioticCodingExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'antibioticcodingexistsquery', 'TableName': 'AntibioticCoding', 'Type': 'Count',
                        'ParameterMapping': 'antibioticcodingexistsmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'AntibioticId', 'Comparison': 'equals' },
                            {'Field': 'CodingId', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
