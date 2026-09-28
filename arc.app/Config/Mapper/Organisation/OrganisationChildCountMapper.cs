using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class OrganisationChildCountMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'organisationusercountmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'organisationchildcount', 
                            'Parameters': [ 
                                {'Key': 'ParentOrganisationId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
