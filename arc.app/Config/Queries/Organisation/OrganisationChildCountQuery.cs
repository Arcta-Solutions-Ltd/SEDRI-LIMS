using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OrganisationChildCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'organisationchildcount', 'TableName': 'Organisation', 'Type': 'Count',
                        'ParameterMapping': 'organisationchildcountmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'ParentOrganisationId', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
