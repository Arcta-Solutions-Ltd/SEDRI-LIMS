using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OrganisationSpecimenCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'organisationspecimencount', 'TableName': 'Specimen', 'Type': 'Count',
                        'ParameterMapping': 'organisationspecimencountmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'OrganisationId', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
