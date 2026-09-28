using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class SourceListExistsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'sourcelistexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            'Name': 'sourcelistexists', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '100'},
                                {'Key': 'Value', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
