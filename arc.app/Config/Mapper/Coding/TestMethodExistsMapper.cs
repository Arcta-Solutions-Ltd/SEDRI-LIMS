using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class TestMethodExistsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'testmethodexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            'Name': 'languageexists', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '79'},
                                {'Key': 'Value', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
