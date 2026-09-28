using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OrganisationByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'OrganisationById', 'TableName': 'Organisation', 'Type': 'Single', 
                        'Fields': [
                            {'Name': 'OrganisationName', 'Type': 'string'},
                            {'Name': 'FullyQualifiedName', 'Type': 'string'},
                            {'Name': 'Code', 'Type': 'string'},
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'ParentOrganisationId', 'Type': 'int'},
                            {'Name': 'LanguageId', 'Type': 'int'},
                            {'Name': 'LocationId', 'Type': 'int'},
                            {'Name': 'Enabled', 'Type': 'string'},
                            {'Name': 'AddressLine1', 'Type': 'string'},
                            {'Name': 'AddressLine2', 'Type': 'string'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ]
                    }";
        }
    }
}
