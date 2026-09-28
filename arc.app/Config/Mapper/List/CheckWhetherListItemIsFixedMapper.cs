using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class CheckWhetherListItemIsFixedMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'checkwhetherlistitemisfixedmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'checkwhetherlistitemisfixed', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '77'},
                                {'Key': 'Id', 'Value': '<:1:>'},
                                {'Key': 'Fixed', 'Value': true }
                            ] 
                        }
                     }";
        }
    }
}
