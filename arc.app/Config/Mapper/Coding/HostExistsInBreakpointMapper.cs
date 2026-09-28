using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class HostExistsInBreakpointMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'hostexistsinbreakpointmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'hostexistsinbreakpointmapper', 
                            'Parameters': [ 
                                {'Key': 'HostId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
