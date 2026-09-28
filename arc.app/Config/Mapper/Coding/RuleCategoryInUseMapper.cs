using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class RuleCategoryInUseMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'rulecategoryinusemapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'rulecategoryinusemapper', 
                            'Parameters': [ 
                                {'Key': 'RuleCategoryId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
