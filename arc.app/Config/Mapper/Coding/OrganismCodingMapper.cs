using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class OrganismCodingMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'organismcodingexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'OrganismId', Value: 'OrganismId' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'MetafCodingId', Value: 'MetafCodingId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'Code', Value: 'Code' }
                        ],
                        'Target' : { 
                            'Name': 'organismcoding', 
                            'Parameters': [ 
                                {'Key': 'OrganismId', 'Value': '<:1:>' },
                                {'Key': 'CodingId', 'Value': '<:2:>' },
                                {'Key': 'Code', 'Value': '<:3:>' }
                            ] 
                        }
                     }";
        }
    }
}
