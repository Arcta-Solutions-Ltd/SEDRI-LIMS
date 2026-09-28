using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OrganisationUserCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'organisationusercount', 'TableName': 'OrganisationUser', 'Type': 'Count',
                        'ParameterMapping': 'organisationusercountmapper',
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
