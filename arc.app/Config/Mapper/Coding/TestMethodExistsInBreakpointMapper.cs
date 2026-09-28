using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class TestMethodExistsInBreakpointMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'testmethodexistsinbreakpointmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'testmethodexistsinbreakpointmapper', 
                            'Parameters': [ 
                                {'Key': 'TestMethodId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
