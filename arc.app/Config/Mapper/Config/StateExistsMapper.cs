using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class StateExistsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'stateexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            'Name': 'stateexists', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '60'},
                                {'Key': 'Value', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
