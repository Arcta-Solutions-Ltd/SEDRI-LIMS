using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class OrganisationUserCountMapper : IDefinition
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
                            'Name': 'organisationusercount', 
                            'Parameters': [ 
                                {'Key': 'OrganisationId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
