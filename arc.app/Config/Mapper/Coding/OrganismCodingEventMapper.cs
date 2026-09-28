using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class OrganismCodingEventMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'organismcodingeventmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'OrganismId', Value: 'OrganismId' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'MetafCodingId', Value: 'MetafCodingId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'Code', Value: 'Code' }
                        ],
                        'Target' : { 
                            'OrganismId': '<:1:>',
                            'CodingId': '<:2:>',
                            'Code': '<:3:>'
                         }
                     }";
        }
    }
}
