using arc.app.Common;

namespace arc.app.Config.Queries;

internal class OrganisationListQuery : IDefinition
{
    public string Get()
    {
        return @"{  
                        'Query': 'organisationlist', 'TableName': 'Organisation', 'Type': 'Select', 'OrderBy' : 'OrganisationName',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'OrganisationName', 'Type': 'string'},
                            {'Name': 'Code', 'Type': 'string'},
                            {'Name': 'FullyQualifiedName', 'Type': 'string' },
                            {'Name': 'Enabled', 'Type': 'string' },
                            {'Name': 'LastModifiedDate', 'Type': 'string'},
                            {'Name': 'ParentOrganisationId', 'Type': 'int'}
                        ],
                        'Joins': [
                            { 'Table': 'Organisation', 'Fields': [{'Name': 'OrganisationName', 'KnownAs': 'ParentOrganisation' }], Type: 'Left', 'On': 'ParentOrganisationId' }
                        ],
                        'Where' : [
                            {'Field': 'OrganisationName', 'Comparison': 'contains', orGroup: 'search' },
                            {'Field': 'Code', 'Comparison': 'contains', orGroup: 'search' },
                            {'Field': 'FullyQualifiedName', 'Comparison': 'contains', orGroup: 'search' }
                        ],
                        'Orderby': 'OrganisationName',
                        'Descending': false
                    }";
    }
}
