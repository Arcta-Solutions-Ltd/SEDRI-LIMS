using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class LanguageExistsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'languageexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            'Name': 'languageexists', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '76'},
                                {'Key': 'Value', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
