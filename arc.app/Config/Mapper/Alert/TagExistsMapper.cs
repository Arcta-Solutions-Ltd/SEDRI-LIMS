using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class TagExistsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'tagexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            'Name': 'tagexists', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '105'},
                                {'Key': 'Value', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
