using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class LaboratoryUserCountMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'laboratoryusercountmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'laboratoryusercount', 
                            'Parameters': [ 
                                {'Key': 'LaboratoryId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
