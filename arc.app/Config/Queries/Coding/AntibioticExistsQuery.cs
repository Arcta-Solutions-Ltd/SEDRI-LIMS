using arc.app.Common;

namespace arc.app.Config.Queries.Coding
{
    internal class AntibioticExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'antibioticexistsquery', 'TableName': 'Antibiotic', 'Type': 'Count',
                        'ParameterMapping': 'antibioticexistsmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
