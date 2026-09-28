using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AntibioticCodingExistsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'antibioticcodingexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'AntibioticId', Value: 'AntibioticId' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'MetafCodingId', Value: 'MetafCodingId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'Code', Value: 'Code' }
                        ],
                        'Target' : { 
                            'Name': 'antibioticcoding', 
                            'Parameters': [ 
                                {'Key': 'AntibioticId', 'Value': '<:1:>' },
                                {'Key': 'CodingId', 'Value': '<:2:>' },
                                {'Key': 'Code', 'Value': '<:3:>' }
                            ] 
                        }
                     }";
        }
    }
}
