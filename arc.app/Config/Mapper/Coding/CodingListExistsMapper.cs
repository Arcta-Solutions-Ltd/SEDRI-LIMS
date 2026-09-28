using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class CodingListExistsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'codinglistexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            'Name': 'codinglistexists', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '77'},
                                {'Key': 'Value', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
