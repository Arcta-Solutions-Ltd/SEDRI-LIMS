using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class TestListForCultureParameterMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'testlistforcultureparametermapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Key', Where: 'Id', Value: 'Value'},
                        ],
                        'Target' : { 
                            'Name': '<:1:>', 
                            'Parameters': [ 
                                {'Key': 'CultureId', 'Value': '<:2:>'}
                            ] 
                        }
                     }";
        }
    }
}
