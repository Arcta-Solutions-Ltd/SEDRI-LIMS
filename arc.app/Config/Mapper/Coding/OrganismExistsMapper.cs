using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class OrganismExistsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'organismexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'OrganismId', Value: 'OrganismId' }
                        ],
                        'Target' : { 
                            'Name': 'organismexists', 
                            'Parameters': [ 
                                {'Key': 'Id', 'Value': '<:1:>' }
                            ] 
                        }
                     }";
        }
    }
}
