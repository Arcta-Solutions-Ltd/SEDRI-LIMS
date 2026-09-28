using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class OrganisationSpecimenCountMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'organisationspecimencountmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'organisationspecimencount', 
                            'Parameters': [ 
                                {'Key': 'OrganisationId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
