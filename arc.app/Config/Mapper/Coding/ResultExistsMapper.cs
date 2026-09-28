using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class ResultExistsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'resultexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            'Name': 'resultexists', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '80'},
                                {'Key': 'Value', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
