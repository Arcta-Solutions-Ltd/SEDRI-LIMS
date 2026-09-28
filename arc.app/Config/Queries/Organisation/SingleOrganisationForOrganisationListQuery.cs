using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SingleOrganisationForOrganisationListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SingleOrganisationForOrganisationList',
                        'TableName': 'Organisation',
                        'Type': 'Single',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'OrganisationName', 'Type': 'string'},
                            {'Name': 'Code', 'Type': 'string'},
                            {'Name': 'FullyQualifiedName', 'Type': 'string' },
                            {'Name': 'Enabled', 'Type': 'string' },
                            {'Name': 'LastModifiedDate', 'Type': 'string'}
                        ],
                        'Joins': [
                            { 'Table': 'Organisation', 'Fields': [{'Name': 'OrganisationName', 'KnownAs': 'ParentOrganisation' }], Type: 'Left', 'On': 'ParentOrganisationId' }
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ]
                    }";
        }
    }
}
