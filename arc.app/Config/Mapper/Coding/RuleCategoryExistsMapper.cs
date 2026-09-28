using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class RuleCategoryExistsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'rulecategoryexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            'Name': 'rulecategoryexists', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '129'},
                                {'Key': 'Value', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
