using arc.app.Common;

namespace arc.app.Config.Mapper
{ 
    internal class AntibioticExistsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'antibioticexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'AntibioticId', Value: 'AntibioticId' }
                        ],
                        'Target' : { 
                            'Name': 'antibioticexistsquery', 
                            'Parameters': [ 
                                {'Key': 'Id', 'Value': '<:1:>' }
                            ] 
                        }
                     }";
        }
    }
}
