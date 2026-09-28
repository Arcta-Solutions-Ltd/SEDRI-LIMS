using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class TestListForSpecimenParameterMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'testlistforspecimenparametermapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Key', Where: 'Id', Value: 'Value'},
                        ],
                        'Target' : { 
                            'Name': '<:1:>', 
                            'Parameters': [ 
                                {'Key': 'SpecimenId', 'Value': '<:2:>'}
                            ] 
                        }
                     }";
        }
    }
}
