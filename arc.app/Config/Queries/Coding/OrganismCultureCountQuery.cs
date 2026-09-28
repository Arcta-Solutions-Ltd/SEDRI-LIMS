using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OrganismCultureCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'organismculturecount', 'TableName': 'Culture', 'Type': 'special'
                    }";
        }
    }
}


//return @"{  
//                        'Query': 'organismculturecount', 'TableName': 'Culture', 'Type': 'Count',
//                        'ParameterMapping': 'organismculturecountmapper',
//                        'Fields': [
//                            {'Name': 'Id', 'Type': 'int'}
//                        ],
//                        'Where' : [
//                            {'Field': 'SpecimenOrganismId', 'Comparison': 'equals' }
//                        ]
//                    }";