using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OrganismExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'organismexists', 'TableName': 'Organism', 'Type': 'Count',
                        'ParameterMapping': 'organismexistsmapper',
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
