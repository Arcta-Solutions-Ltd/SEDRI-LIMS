using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AlertTypeUsedInAlertMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'alerttypeusedinalertmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'alerttypeusedinalert', 
                            'Parameters': [ 
                                {'Key': 'AlertTypeId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
