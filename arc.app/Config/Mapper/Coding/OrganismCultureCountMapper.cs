using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class OrganismCultureCountMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'organismculturecountmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'organismculturecount', 
                            'Parameters': [ 
                                {'Key': 'SpecimenOrganismId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
