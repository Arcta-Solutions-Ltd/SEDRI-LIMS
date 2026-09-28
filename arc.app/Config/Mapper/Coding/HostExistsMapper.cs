using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class HostExistsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'hostexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            'Name': 'hostexists', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '78'},
                                {'Key': 'Value', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
