using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AntibioticCodingEventMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'antibioticcodingeventmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'AntibioticId', Value: 'AntibioticId' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'MetafCodingId', Value: 'MetafCodingId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'Code', Value: 'Code' }
                        ],
                        'Target' : { 
                            'AntibioticId': '<:1:>',
                            'CodingId': '<:2:>',
                            'Code': '<:3:>'
                         }
                     }";
        }
    }
}
