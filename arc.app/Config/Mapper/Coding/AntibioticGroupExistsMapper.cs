using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AntibioticGroupExistsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'antibioticgroupexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            'Name': 'antibioticgroupexistsquery', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '82'},
                                {'Key': 'Value', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
