using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class CheckWhetherTableEntryIsFixedMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'checkwhethertableentryisfixedmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'checkwhethertableentryisfixed', 
                            'Parameters': [ 
                                {'Key': 'Id', 'Value': '<:1:>'},
                                {'Key': 'Fixed', 'Value': true }
                            ] 
                        }
                     }";
        }
    }
}
